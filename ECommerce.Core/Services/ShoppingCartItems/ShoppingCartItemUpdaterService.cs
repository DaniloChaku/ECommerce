using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItems
{
    /// <summary>
    /// Service for updating shopping cart items.
    /// </summary>
    public class ShoppingCartItemUpdaterService : IShoppingCartItemUpdaterService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IProductGetterService _productGetterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemUpdaterService"/> class.
        /// </summary>
        /// <param name="shoppingCartItemRepository">The repository for interacting with shopping cart items.</param>
        /// <param name="productGetterService">The service for retrieving product information.</param>
        public ShoppingCartItemUpdaterService(IShoppingCartItemRepository shoppingCartItemRepository,
            IProductGetterService productGetterService)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _productGetterService = productGetterService;
        }

        /// <summary>
        /// Updates a shopping cart item with the provided data.
        /// </summary>
        /// <param name="shoppingCartItemDto">The data for the shopping cart item to be updated.</param>
        /// <returns>The updated shopping cart item.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided shopping cart item data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the ID of the shopping cart item is empty, 
        /// when the product ID is invalid, or when the shopping cart item does not exist.</exception>
        /// <exception cref="QuantityExceedsStockException">Thrown when the count of the shopping cart item 
        /// exceeds the available stock.</exception>
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
