using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository interface for managing shopping cart items.
    /// </summary>
    public interface IShoppingCartItemRepository : IRepository<ShoppingCartItem>
    {
        /// <summary>
        /// Updates a shopping cart item asynchronously in the database.
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated shopping cart item.</returns>
        Task<ShoppingCartItem> UpdateAsync(ShoppingCartItem shoppingCartItem);
    }
}
