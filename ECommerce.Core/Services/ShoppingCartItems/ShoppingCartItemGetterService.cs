using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;

namespace ECommerce.Core.Services.ShoppingCartItems
{
    /// <summary>
    /// Service for retrieving shopping cart items.
    /// </summary>
    public class ShoppingCartItemGetterService : IShoppingCartItemGetterService
    {
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartItemGetterService"/> class.
        /// </summary>
        /// <param name="shoppingCartItemRepository">The repository for interacting with shopping cart items.</param>
        public ShoppingCartItemGetterService(IShoppingCartItemRepository shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }

        /// <summary>
        /// Retrieves all shopping cart items.
        /// </summary>
        /// <returns>A list of all shopping cart items.</returns>
        public async Task<List<ShoppingCartItemDto>> GetAllAsync()
        {
            var shoppingCartItems = await _shoppingCartItemRepository.GetAllAsync();

            return shoppingCartItems.Select(i => i.ToDto()).ToList();
        }

        /// <summary>
        /// Retrieves a shopping cart item by customer ID and product ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>The shopping cart item matching the customer and product IDs, or null if not found.</returns>
        public async Task<ShoppingCartItemDto?> GetByCustomerAndProductIdAsync(Guid customerId, Guid productId)
        {
            var shoppingCartItems = await _shoppingCartItemRepository.GetAllAsync(i =>
            i.CustomerId == customerId && i.ProductId == productId);

            return shoppingCartItems.FirstOrDefault()?.ToDto();
        }

        /// <summary>
        /// Retrieves shopping cart items by customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>A list of shopping cart items belonging to the specified customer.</returns>
        public async Task<List<ShoppingCartItemDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var shoppingCartItems = await _shoppingCartItemRepository.GetAllAsync(i => i.CustomerId == customerId);

            return shoppingCartItems.Select(i => i.ToDto()).ToList();
        }

        /// <summary>
        /// Retrieves a shopping cart item by its ID.
        /// </summary>
        /// <param name="id">The ID of the shopping cart item to retrieve.</param>
        /// <returns>The shopping cart item with the specified ID, or null if not found.</returns>
        public async Task<ShoppingCartItemDto?> GetByIdAsync(Guid id)
        {
            var shoppingCartItem = await _shoppingCartItemRepository.GetByIdAsync(id);

            return shoppingCartItem?.ToDto();
        }
    }
}
