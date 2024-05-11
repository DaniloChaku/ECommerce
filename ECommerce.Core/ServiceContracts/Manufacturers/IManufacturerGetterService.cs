using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    /// <summary>
    /// Defines the contract for retrieving manufacturers.
    /// </summary>
    public interface IManufacturerGetterService
    {
        /// <summary>
        /// Retrieves all manufacturers asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of manufacturers.</returns>
        Task<List<ManufacturerDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a manufacturer by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the manufacturer to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the manufacturer if found; otherwise, null.
        /// </returns>
        Task<ManufacturerDto?> GetByIdAsync(Guid id);
    }
}
