using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    public interface IShoppingCartItemUpdaterService
    {
        Task<ShoppingCartItemDto> UpdateAsync(ShoppingCartItemDto shoppingCartItemDto);
    }
}
