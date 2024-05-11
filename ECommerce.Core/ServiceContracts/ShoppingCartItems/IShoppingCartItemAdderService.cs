using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    public interface IShoppingCartItemAdderService
    {
        Task<ShoppingCartItemDto> AddAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
}
