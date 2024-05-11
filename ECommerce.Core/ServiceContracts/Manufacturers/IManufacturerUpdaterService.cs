using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    /// <summary>
    /// Defines the contract for updating a manufacturer.
    /// </summary>
    public interface IManufacturerUpdaterService
    {
        /// <summary>
        /// Updates a manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerDto">The data transfer object representing the manufacturer to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated manufacturer.</returns>
        Task<ManufacturerDto> UpdateAsync(ManufacturerDto manufacturerDto);
    }
}
