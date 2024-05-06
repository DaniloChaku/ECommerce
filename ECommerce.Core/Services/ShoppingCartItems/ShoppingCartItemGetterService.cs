using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItem
{
    public class ShoppingCartItemGetterService : IShoppingCartItemGetterService
    {
        private readonly IShoppingCartItemRepository _shoppingartItemRepository;

        public ShoppingCartItemGetterService(IShoppingCartItemRepository shoppingcartitemRepository)
        {
            _shoppingartItemRepository = shoppingcartitemRepository;
        }

        public async Task<List<ShoppingCartItemDto>> GetAllAsync()
        {
            var categories = await _shoppingartItemRepository.GetAllAsync();

            return categories.Select(t => t.ToDto()).ToList();
        }

        public async Task<ShoppingCartItemDto?> GetByIdAsync(Guid id)
        {
            var shoppingcartitem = await _shoppingartItemRepository.GetByIdAsync(id);

            return shoppingcartitem?.ToDto();
        }
    }
}
