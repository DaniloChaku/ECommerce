using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItem
{
    public class ShoppingCartItemDeleterService : IShoppingCartItemDeleterService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        public ShoppingCartItemDeleterService(IShoppingCartItemRepository shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }

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
