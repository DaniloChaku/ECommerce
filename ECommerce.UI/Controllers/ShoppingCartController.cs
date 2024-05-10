using ECommerce.Core.Exceptions;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.ServiceContracts.Users;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.UI.Controllers
{
    [Authorize(Roles = Constants.ROLE_CUSTOMER)]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IShoppingCartItemDeleterService _shoppingCartItemDeleterService;
        private readonly IShoppingCartItemUpdaterService _shoppingCartItemUpdaterService;
        private readonly IUserContextService _userContextService;

        public ShoppingCartController(IShoppingCartItemGetterService shoppingCartItemGetterService,
            IShoppingCartItemDeleterService shoppingCartItemDeleterService,
            IShoppingCartItemUpdaterService shoppingCartItemUpdaterService,
            IUserContextService userContextService)
        {
            _shoppingCartItemGetterService = shoppingCartItemGetterService;
            _shoppingCartItemDeleterService = shoppingCartItemDeleterService;
            _shoppingCartItemUpdaterService = shoppingCartItemUpdaterService;
            _userContextService = userContextService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customerId = _userContextService.GetCustomerId(User.Identity as ClaimsIdentity);

            var items = await _shoppingCartItemGetterService.GetByCustomerIdAsync(customerId!.Value);
            decimal totalPrice = items.Sum(i => i.Count * i.ProductPrice!.Value);

            var shoppingCartViewModel = new ShoppingCartViewModel()
            {
                Items = items,
                TotalPrice = totalPrice
            };

            return View(shoppingCartViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Minus(Guid id)
        {
            var cart = await _shoppingCartItemGetterService.GetByIdAsync(id);

            if (cart is null)
            {
                TempData["error"] = "Product not found.";
            }
            else
            {
                if (cart.Count == 1)
                {
                    await _shoppingCartItemDeleterService.DeleteAsync(id);
                }
                else
                {
                    cart.Count -= 1;
                    await _shoppingCartItemUpdaterService.UpdateAsync(cart);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Plus(Guid id)
        {
            var cart = await _shoppingCartItemGetterService.GetByIdAsync(id);

            if (cart is null)
            {
                TempData["error"] = "Product not found.";
            }
            else
            {
                cart.Count += 1;
                try
                {
                    await _shoppingCartItemUpdaterService.UpdateAsync(cart);
                }
                catch (QuantityExceedsStockException ex)
                {
                    TempData["error"] = ex.Message;
                }
                catch (Exception)
                {
                    TempData["error"] = "An error occurred. Please, try again later.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var cart = await _shoppingCartItemGetterService.GetByIdAsync(id);

            if (cart is null)
            {
                TempData["error"] = "Product not found.";
            }
            else
            {
                try
                {
                    await _shoppingCartItemDeleterService.DeleteAsync(id);
                }
                catch (Exception)
                {
                    TempData["error"] = "An error occurred. Please, try again later.";
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
