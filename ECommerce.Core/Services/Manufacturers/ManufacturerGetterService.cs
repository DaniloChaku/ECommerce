using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    /// <summary>
    /// Service for retrieving manufacturers.
    /// </summary>
    public class ManufacturerGetterService : IManufacturerGetterService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerGetterService"/> class.
        /// </summary>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        public ManufacturerGetterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        /// <summary>
        /// Retrieves all manufacturers asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. A list of all manufacturers.</returns>
        public async Task<List<ManufacturerDto>> GetAllAsync()
        {
            var manufacturers = await _manufacturerRepository.GetAllAsync();

            return manufacturers.Select(m => m.ToDto()).ToList();
        }

        /// <summary>
        /// Retrieves a manufacturer by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the manufacturer to retrieve.</param>
        /// <returns>A task representing the asynchronous operation. The manufacturer with the specified ID, or null if not found.</returns>
        public async Task<ManufacturerDto?> GetByIdAsync(Guid id)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id);

            return manufacturer?.ToDto();
        }
    }
}
