namespace ECommerce.Core.ServiceContracts.ShoppingCartItems
{
    /// <summary>
    /// Defines the contract for deleting a shopping cart item.
    /// </summary>
    public interface IShoppingCartItemDeleterService
    {
        /// <summary>
        /// Deletes a shopping cart item asynchronously based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the shopping cart item to delete.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
