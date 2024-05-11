using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItems
{
    /// <summary>
    /// Service for adding shopping cart items.
    /// </summary>
    public class ShoppingCartItemAdderService : IShoppingCartItemAdderService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;
        private readonly IProductGetterService _productGetterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemAdderService"/> class.
        /// </summary>
        /// <param name="shoppingCartItemRepository">The repository for interacting with shopping cart items.</param>
        /// <param name="productGetterService">The service for retrieving product information.</param>
        public ShoppingCartItemAdderService(IShoppingCartItemRepository shoppingCartItemRepository,
            IProductGetterService productGetterService)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
            _productGetterService = productGetterService;
        }

        /// <summary>
        /// Adds a new shopping cart item.
        /// </summary>
        /// <param name="shoppingCartItemDto">The shopping cart item to add.</param>
        /// <returns>The added shopping cart item.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided shopping cart item is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the ID of the shopping cart item is not empty or when the product ID is invalid.</exception>
        /// <exception cref="QuantityExceedsStockException">Thrown when the count of the shopping cart item exceeds the available stock.</exception>
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
