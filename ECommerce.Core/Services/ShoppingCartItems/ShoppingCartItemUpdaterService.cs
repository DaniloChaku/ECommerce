using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItems
{
    public class ShoppingCartItemUpdaterService : IShoppingCartItemUpdaterService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IProductGetterService _productGetterService;

        public ShoppingCartItemUpdaterService(IShoppingCartItemRepository shoppingCartItemRepository,
            IProductGetterService productGetterService)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _productGetterService = productGetterService;
        }

        public async Task<ShoppingCartItemDto> UpdateAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            if (shoppingCartItemDto is null)
            {
                throw new ArgumentNullException(nameof(shoppingCartItemDto), "ShoppingCartItem data cannot be null");
            }

            if (shoppingCartItemDto.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(shoppingCartItemDto.Id));
            }

            var product = await _productGetterService.GetByIdAsync(shoppingCartItemDto.ProductId);
            if (product is null)
            {
                throw new ArgumentException("Invalid Product Id");
            }

            if (product.Stock < shoppingCartItemDto.Count)
            {
                throw new QuantityExceedsStockException("The number of products selected exceeds the number of products available");
            }

            var existingShoppingCartItem = await _shoppingCartItemRepository.GetByIdAsync(shoppingCartItemDto.Id);
            if (existingShoppingCartItem is null)
            {
                throw new ArgumentException("ShoppingCartItem does not exist");
            }

            var shoppingcartitem = shoppingCartItemDto.ToEntity();

            var shoppingcartitemUpdated = await _shoppingCartItemRepository.UpdateAsync(shoppingcartitem);

            return shoppingcartitemUpdated.ToDto();
        }
    }
}
