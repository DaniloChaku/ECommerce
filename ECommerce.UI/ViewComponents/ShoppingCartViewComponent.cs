using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.ServiceContracts.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.UI.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IUserContextService _userContextService;

        public ShoppingCartViewComponent(IShoppingCartItemGetterService shoppingCartItemGetterService,
            IUserContextService userContextService)
        {
            _shoppingCartItemGetterService = shoppingCartItemGetterService;
            _userContextService = userContextService;
        }

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
