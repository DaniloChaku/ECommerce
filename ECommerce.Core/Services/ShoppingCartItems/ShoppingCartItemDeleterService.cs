using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItems
{
    /// <summary>
    /// Service for deleting shopping cart items.
    /// </summary>
    public class ShoppingCartItemDeleterService : IShoppingCartItemDeleterService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemDeleterService"/> class.
        /// </summary>
        /// <param name="shoppingCartItemRepository">The repository for interacting with shopping cart items.</param>
        public ShoppingCartItemDeleterService(IShoppingCartItemRepository shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }

        /// <summary>
        /// Deletes a shopping cart item by its ID.
        /// </summary>
        /// <param name="id">The ID of the shopping cart item to be deleted.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified shopping cart item does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the deletion operation fails.</exception>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingShoppingCartItem = await _shoppingCartItemRepository.GetByIdAsync(id);

            if (existingShoppingCartItem is null)
            {
                throw new ArgumentException("ShoppingCartItem does not exist");
            }

            if (!await _shoppingCartItemRepository.DeleteAsync(existingShoppingCartItem))
            {
                throw new InvalidOperationException("Failed to delete ShoppingCartItem");
            }

            return true;
        }
    }
}
