using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItems
{
    public class ShoppingCartItemGetterService : IShoppingCartItemGetterService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        public ShoppingCartItemGetterService(IShoppingCartItemRepository shoppingcartitemRepository)
        {
            _shoppingCartItemRepository = shoppingcartitemRepository;
        }

        public async Task<List<ShoppingCartItemDto>> GetAllAsync()
        {
            var shoppingCartItems = await _shoppingCartItemRepository.GetAllAsync();

            return shoppingCartItems.Select(i => i.ToDto()).ToList();
        }

        public async Task<ShoppingCartItemDto?> GetByCustomerAndProductIdAsync(Guid customerId, Guid productId)
        {
            var shoppingCartItems = await _shoppingCartItemRepository.GetAllAsync(i =>
            i.CustomerId == customerId && i.ProductId == productId);

            return shoppingCartItems.FirstOrDefault()?.ToDto();
        }

        public async Task<List<ShoppingCartItemDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var shoppingCartItems = await _shoppingCartItemRepository.GetAllAsync(i => i.CustomerId == customerId);

            return shoppingCartItems.Select(i => i.ToDto()).ToList();
        }

        public async Task<ShoppingCartItemDto?> GetByIdAsync(Guid id)
        {
            var shoppingCartItem = await _shoppingCartItemRepository.GetByIdAsync(id);

            return shoppingCartItem?.ToDto();
        }
    }
}
