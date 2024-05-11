using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    /// <summary>
    /// Service for adding manufacturers.
    /// </summary>
    public class ManufacturerAdderService : IManufacturerAdderService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerAdderService"/> class.
        /// </summary>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        public ManufacturerAdderService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        /// <summary>
        /// Adds a manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerDto">The manufacturer DTO containing the data of the manufacturer to add.</param>
        /// <returns>The added manufacturer DTO.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the manufacturer DTO is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the ID is not empty or a manufacturer 
        /// with the same name already exists.</exception>
        public async Task<ManufacturerDto> AddAsync(ManufacturerDto manufacturerDto)
        {
            if (manufacturerDto is null)
            {
                throw new ArgumentNullException(nameof(manufacturerDto), "manufacturer data cannot be null");
            }

            if (manufacturerDto.Id != Guid.Empty)
            {
                throw new ArgumentException("Id must be empty", nameof(manufacturerDto.Id));
            }

            var existingManufacturers = await _manufacturerRepository.GetAllAsync(t => t.Name == manufacturerDto.Name);
            if (existingManufacturers.Any())
            {
                throw new ArgumentException("manufacturer with the same name already exists");
            }

            var manufacturer = manufacturerDto.ToEntity();

            var manufacturerAdded = await _manufacturerRepository.AddAsync(manufacturer);

            return manufacturerAdded.ToDto();
        }
    }
}
