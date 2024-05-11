namespace ECommerce.Core.ServiceContracts.Categories
{
    /// <summary>
    /// Defines the contract for deleting a category.
    /// </summary>
    public interface ICategoryDeleterService
    {
        /// <summary>
        /// Deletes a category asynchronously based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
