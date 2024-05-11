using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    public interface IShoppingCartItemRepository : IRepository<ShoppingCartItem>
    {
        Task<ShoppingCartItem> UpdateAsync(ShoppingCartItem shoppingCartItem);
    }
}
