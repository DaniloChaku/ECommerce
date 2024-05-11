namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    /// <summary>
    /// Defines the contract for deleting a manufacturer.
    /// </summary>
    public interface IManufacturerDeleterService
    {
        /// <summary>
        /// Deletes a manufacturer asynchronously based on its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the manufacturer to delete.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
}
