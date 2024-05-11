using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    /// <summary>
    /// Defines the contract for retrieving shopping cart items.
    /// </summary>
    public interface IShoppingCartItemGetterService
    {
        /// <summary>
        /// Retrieves all shopping cart items asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of shopping cart items.</returns>
        Task<List<ShoppingCartItemDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a shopping cart item by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart item to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the shopping cart item if found; otherwise, null.
        /// </returns>
        Task<ShoppingCartItemDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves shopping cart items by customer identifier asynchronously.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of shopping cart items.</returns>
        Task<List<ShoppingCartItemDto>> GetByCustomerIdAsync(Guid customerId);

        /// <summary>
        /// Retrieves a shopping cart item by customer and product identifiers asynchronously.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the shopping cart item if found; otherwise, null.
        /// </returns>
        Task<ShoppingCartItemDto?> GetByCustomerAndProductIdAsync(Guid customerId, Guid productId);
    }
}
