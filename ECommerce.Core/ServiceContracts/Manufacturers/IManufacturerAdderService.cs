using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    /// <summary>
    /// Defines the contract for adding a manufacturer.
    /// </summary>
    public interface IManufacturerAdderService
    {
        /// <summary>
        /// Adds a new manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerDto">The data transfer object that represents the manufacturer to add.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the added manufacturer.</returns>
        Task<ManufacturerDto> AddAsync(ManufacturerDto manufacturerDto);
    }
}
