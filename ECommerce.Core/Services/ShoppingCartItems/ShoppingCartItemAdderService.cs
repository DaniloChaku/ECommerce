using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItem
{
    public class ShoppingCartItemAdderService : IShoppingCartItemAdderService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IProductGetterService _productGetterService;

        public ShoppingCartItemAdderService(IShoppingCartItemRepository shoppingCartItemRepository,
            IProductGetterService productGetterService)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _productGetterService = productGetterService;
        }

        public async Task<ShoppingCartItemDto> AddAsync(ShoppingCartItemDto shoppingCartItemDto)
        {
            if (shoppingCartItemDto is null)
            {
                throw new ArgumentNullException(nameof(shoppingCartItemDto), "ShoppingCartItem cannot be null");
            }

            if (shoppingCartItemDto.Id != Guid.Empty)
            {
                throw new ArgumentException("Id must be empty", nameof(shoppingCartItemDto.Id));
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

            var shoppingcartitem = shoppingCartItemDto.ToEntity();

            var shoppingcartitemAdded = await _shoppingCartItemRepository.AddAsync(shoppingcartitem);

            return shoppingcartitemAdded.ToDto();
        }
    }
}
