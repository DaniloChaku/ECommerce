using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    /// <summary>
    /// Service for updating manufacturer information.
    /// </summary>
    public class ManufacturerUpdaterService : IManufacturerUpdaterService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerUpdaterService"/> class.
        /// </summary>
        /// <param name="manufacturerRepository">The repository for interacting with manufacturers.</param>
        public ManufacturerUpdaterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        /// <summary>
        /// Updates the information of a manufacturer.
        /// </summary>
        /// <param name="manufacturerDto">The DTO containing the updated manufacturer information.</param>
        /// <returns>The updated manufacturer DTO.</returns>
        public async Task<ManufacturerDto> UpdateAsync(ManufacturerDto manufacturerDto)
        {
            if (manufacturerDto is null)
            {
                throw new ArgumentNullException(nameof(manufacturerDto), "Manufacturer data cannot be null");
            }

            if (manufacturerDto.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(manufacturerDto.Id));
            }

            var existingManufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerDto.Id);
            if (existingManufacturer is null)
            {
                throw new ArgumentException("Manufacturer does not exist");
            }

            var manufacturer = manufacturerDto.ToEntity();

            var manufacturerUpdated = await _manufacturerRepository.UpdateAsync(manufacturer);

            return manufacturerUpdated.ToDto();
        }
    }
}
