using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.ServiceContracts.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.UI.ViewComponents
{
    /// <summary>
    /// View component for displaying the shopping cart.
    /// </summary>
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IUserContextService _userContextService;

        /// <summary>
        /// Initializes a new instance of the ShoppingCartViewComponent class.
        /// </summary>
        /// <param name="shoppingCartItemGetterService">The service for retrieving shopping cart items.</param>
        /// <param name="userContextService">The service for accessing user context.</param>
        public ShoppingCartViewComponent(IShoppingCartItemGetterService shoppingCartItemGetterService,
            IUserContextService userContextService)
        {
            _shoppingCartItemGetterService = shoppingCartItemGetterService;
            _userContextService = userContextService;
        }

        /// <summary>
        /// Invokes the view component asynchronously.
        /// </summary>
        /// <returns>An asynchronous task that returns an IViewComponentResult.</returns>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var customerId = _userContextService.GetCustomerId(User.Identity as ClaimsIdentity);

            if (customerId is not null)
            {
                var shoppingCartItems = await _shoppingCartItemGetterService
                    .GetByCustomerIdAsync(customerId.Value);

                return View(shoppingCartItems.Count);
            }

            return View(0);
        }
    }
}
