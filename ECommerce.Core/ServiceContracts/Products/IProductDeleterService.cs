namespace ECommerce.Core.ServiceContracts.Products
{
    /// <summary>
    /// Defines the contract for deleting a product.
    /// </summary>
    public interface IProductDeleterService
    {
        /// <summary>
        /// Deletes a product asynchronously based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
