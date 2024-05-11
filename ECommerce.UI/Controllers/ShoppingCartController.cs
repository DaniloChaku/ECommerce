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
    /// <summary>
    /// Controller responsible for managing shopping cart-related actions, such as adding, 
    /// removing, and updating items in the shopping cart.
    /// </summary>
    [Authorize(Roles = Constants.ROLE_CUSTOMER)]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IShoppingCartItemDeleterService _shoppingCartItemDeleterService;
        private readonly IShoppingCartItemUpdaterService _shoppingCartItemUpdaterService;
        private readonly IUserContextService _userContextService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartController"/> class.
        /// </summary>
        /// <param name="shoppingCartItemGetterService">The shopping cart item getter service.</param>
        /// <param name="shoppingCartItemDeleterService">The shopping cart item deleter service.</param>
        /// <param name="shoppingCartItemUpdaterService">The shopping cart item updater service.</param>
        /// <param name="userContextService">The user context service.</param>
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

        /// <summary>
        /// Displays the shopping cart with all items added by the current customer.
        /// </summary>
        /// <returns>The shopping cart view.</returns>
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

        /// <summary>
        /// Decreases the quantity of a product in the shopping cart and redirects to the shopping cart page.
        /// </summary>
        /// <param name="id">The ID of the shopping cart item to decrease.</param>
        /// <returns>A redirect to the shopping cart page.</returns>
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

        /// <summary>
        /// Increases the quantity of a product in the shopping cart and redirects to the shopping cart page.
        /// </summary>
        /// <param name="id">The ID of the shopping cart item to increase.</param>
        /// <returns>A redirect to the shopping cart page.</returns>
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
                    TempData["error"] = Constants.GENERIC_ERROR_MESSAGE;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Removes a product from the shopping cart and redirects to the shopping cart page.
        /// </summary>
        /// <param name="id">The ID of the shopping cart item to remove.</param>
        /// <returns>A redirect to the shopping cart page.</returns>
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
                    TempData["error"] = Constants.GENERIC_ERROR_MESSAGE;
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
