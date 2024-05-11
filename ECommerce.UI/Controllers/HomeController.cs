using ECommerce.Core.Dtos;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.ServiceContracts.Users;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerce.UI.Controllers
{
    /// <summary>
    /// Controller responsible for handling home-related actions such as displaying products, 
    /// product details, and managing the shopping cart.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IProductGetterService _productGetterService;
        private readonly IShoppingCartItemAdderService _shoppingCartItemAdderService;
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IShoppingCartItemUpdaterService _shoppingCartItemUpdaterService;
        private readonly IUserContextService _userContextService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="productGetterService">The product getter service.</param>
        /// <param name="shoppingCartItemAdderService">The shopping cart item adder service.</param>
        /// <param name="shoppingCartItemGetterService">The shopping cart item getter service.</param>
        /// <param name="shoppingCartItemUpdaterService">The shopping cart item updater service.</param>
        /// <param name="userContextService">The user context service.</param>
        public HomeController(IProductGetterService productGetterService,
            IShoppingCartItemAdderService shoppingCartItemAdderService,
            IShoppingCartItemGetterService shoppingCartItemGetterService,
            IShoppingCartItemUpdaterService shoppingCartItemUpdaterService,
            IUserContextService userContextService)
        {
            _productGetterService = productGetterService;
            _shoppingCartItemAdderService = shoppingCartItemAdderService;
            _shoppingCartItemGetterService = shoppingCartItemGetterService;
            _shoppingCartItemUpdaterService = shoppingCartItemUpdaterService;
            _userContextService = userContextService;
        }

        /// <summary>
        /// Displays the home page with a list of products.
        /// </summary>
        /// <param name="page">The page number for pagination.</param>
        /// <param name="productPage">The product page view model containing search query and pagination data.</param>
        /// <returns>The home page view.</returns>
        public async Task<IActionResult> Index(int page = 1, ProductPageViewModel? productPage = null)
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

            var productPageModel = new ProductPageViewModel()
            {
                Products = productsForDisplay,
                CurrentPage = currentPage,
                PaginationStart = paginationStart,
                PaginationEnd = paginationEnd,
                TotalPages = totalPages
            };

            return View(productPageModel);
        }

        /// <summary>
        /// Displays the details of a product.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product detail view.</returns>
        public async Task<IActionResult> Detail(Guid id)
        {
            var product = await _productGetterService.GetByIdAsync(id);

            if (product is null)
            {
                TempData["error"] = "Product not found.";
                return RedirectToAction("Index");
            }

            var productDetailsViewModel = new ProductDetailViewModel()
            {
                Product = product
            };

            var customerId = _userContextService.GetCustomerId(User.Identity as ClaimsIdentity);

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

        /// <summary>
        /// Handles the submission of product details, updates the shopping cart, and redirects to the home page.
        /// </summary>
        /// <param name="model">The product detail view model containing product details and shopping cart information.</param>
        /// <returns>A redirect to the home page.</returns>
        [Authorize(Roles = Constants.ROLE_CUSTOMER)]
        [HttpPost]
        public async Task<IActionResult> Detail(ProductDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerId = _userContextService.GetCustomerId(User.Identity as ClaimsIdentity);

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
                    TempData["success"] = "The quantity of the product in your shopping cart has been updated successfully.";
                }
                else
                {
                    await _shoppingCartItemAdderService.AddAsync(shoppingCartItem);
                    TempData["success"] = "The product was successfully added to your shopping cart.";
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
                TempData["error"] = Constants.GENERIC_ERROR_MESSAGE;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Adds a product to the shopping cart and redirects to the home page.
        /// </summary>
        /// <param name="id">The ID of the product to add.</param>
        /// <param name="currentPage">The current page number for pagination.</param>
        /// <returns>A redirect to the home page.</returns>
        public async Task<IActionResult> Buy(Guid id, int currentPage)
        {
            var customerId = _userContextService.GetCustomerId(User.Identity as ClaimsIdentity);

            var cart = await _shoppingCartItemGetterService
                .GetByCustomerAndProductIdAsync(customerId!.Value, id);

            try
            {
                if (cart is null)
                {
                    var shoppingCartItem = new ShoppingCartItemDto()
                    {
                        ProductId = id,
                        CustomerId = customerId!.Value,
                        Count = 1
                    };

                    await _shoppingCartItemAdderService.AddAsync(shoppingCartItem);
                    TempData["success"] = "The product was successfully added to your shopping cart.";

                }
                else
                {
                    cart.Count += 1;
                    await _shoppingCartItemUpdaterService.UpdateAsync(cart);
                    TempData["success"] = "The product was successfully added to your shopping cart.";

                }
            }
            catch (QuantityExceedsStockException ex)
            {
                TempData["error"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["error"] = Constants.GENERIC_ERROR_MESSAGE;
            }


            return RedirectToAction(nameof(Index), new { page = currentPage });
        }

        /// <summary>
        /// Displays the privacy policy page.
        /// </summary>
        /// <returns>The privacy policy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Handles errors and displays the error page.
        /// </summary>
        /// <returns>The error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
