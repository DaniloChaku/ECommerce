using ECommerce.Core.Dtos;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace ECommerce.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductGetterService _productGetterService;
        private readonly IShoppingCartItemAdderService _shoppingCartItemAdderService;
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IShoppingCartItemUpdaterService _shoppingCartItemUpdaterService;

        public HomeController(IProductGetterService productGetterService,
            IShoppingCartItemAdderService shoppingCartItemAdderService,
            IShoppingCartItemGetterService shoppingCartItemGetterService,
            IShoppingCartItemUpdaterService shoppingCartItemUpdaterService)
        {
            _productGetterService = productGetterService;
            _shoppingCartItemAdderService = shoppingCartItemAdderService;
            _shoppingCartItemGetterService = shoppingCartItemGetterService;
            _shoppingCartItemUpdaterService = shoppingCartItemUpdaterService;
        }

        public async Task<IActionResult> Index(int page = 1, ProductPageModel? productPage = null)
        {
            var products = await _productGetterService
                .GetBySearchQueryAsync(productPage?.SearchQuery);
            var productsPerPage = 10;
            int totalPages;

            if (products.Count != 0 && products.Count % productsPerPage == 0)
            {
                totalPages = products.Count / productsPerPage;
            }
            else
            {
                totalPages = products.Count / productsPerPage + 1;
            }

            var currentPage = (page < 1 || page > totalPages) ? 1 : page;
            var adjacentPagesCount = 2;
            var paginationStart = Math.Max(currentPage - adjacentPagesCount, 1);
            var paginationEnd = Math.Min(currentPage + adjacentPagesCount, totalPages);

            var pageStartIndex = (currentPage - 1) * productsPerPage;
            var productsOnCurrentPage = Math.Min(products.Count - pageStartIndex, 10);
            var productsForDisplay = products.GetRange(pageStartIndex, productsOnCurrentPage);

            var productPageModel = new ProductPageModel()
            {
                Products = productsForDisplay,
                CurrentPage = currentPage,
                PaginationStart = paginationStart,
                PaginationEnd = paginationEnd,
                TotalPages = totalPages
            };

            return View(productPageModel);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await _productGetterService.GetByIdAsync(id);

            if (product is null)
            {
                TempData["error"] = "Product not found";
                return RedirectToAction("Index");
            }

            var productDetailsViewModel = new ProductDetailViewModel()
            {
                Product = product
            };

            var customerId = GetCustomerId();

            if (customerId is not null)
            {
                var existingShoppingCartItem = await _shoppingCartItemGetterService.
                    GetByCustomerAndProductIdAsync(customerId.Value, id);

                if (existingShoppingCartItem is not null)
                {
                    productDetailsViewModel.ShoppingCartItemId = existingShoppingCartItem.Id;
                    productDetailsViewModel.Count = existingShoppingCartItem.Count;
                }
            }

            return View(productDetailsViewModel);
        }

        [Authorize(Roles = Constants.ROLE_CUSTOMER)]
        [HttpPost]
        public async Task<IActionResult> Detail(ProductDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerId = GetCustomerId();

            var shoppingCartItem = new ShoppingCartItemDto()
            {
                ProductId = model.Product.Id,
                CustomerId = customerId!.Value,
                Count = model.Count
            };

            try
            {
                if (model.ShoppingCartItemId is not null)
                {
                    shoppingCartItem.Id = model.ShoppingCartItemId.Value;
                    await _shoppingCartItemUpdaterService.UpdateAsync(shoppingCartItem);
                    TempData["success"] = "The product in your shopping cart was updated successfully";
                }
                else
                {
                    await _shoppingCartItemAdderService.AddAsync(shoppingCartItem);
                    TempData["success"] = "The product was successfully added to your shopping cart";
                }
            }
            catch (QuantityExceedsStockException ex)
            {
                model.Product = await _productGetterService.GetByIdAsync(model.Product.Id);
                ModelState.AddModelError(nameof(shoppingCartItem.Count),
                    ex.Message);
                return View(model);
            }
            catch (Exception)
            {
                model.Product = await _productGetterService.GetByIdAsync(model.Product.Id);
                TempData["error"] = "An error occurred. Please, try again later";
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Guid? GetCustomerId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var cutomerId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (cutomerId is null)
            {
                return null;
            }

            return new Guid(cutomerId);
        }
    }
}
