using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    public interface IShoppingCartItemGetterService
    {
        Task<List<ShoppingCartItemDto>> GetAllAsync();
        Task<ShoppingCartItemDto?> GetByIdAsync(Guid id);
        Task<List<ShoppingCartItemDto>> GetByCustomerIdAsync(Guid customerId);
        Task<ShoppingCartItemDto?> GetByCustomerAndProductIdAsync(Guid customerId, Guid productId);
    }
}
