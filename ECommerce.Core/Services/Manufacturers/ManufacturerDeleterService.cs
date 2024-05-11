using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    /// <summary>
    /// Service for deleting manufacturers.
    /// </summary>
    public class ManufacturerDeleterService : IManufacturerDeleterService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerDeleterService"/> class.
        /// </summary>
        /// <param name="manufacturerRepository">The manufacturer repository.</param>
        public ManufacturerDeleterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        /// <summary>
        /// Deletes a manufacturer asynchronously.
        /// </summary>
        /// <param name="id">The ID of the manufacturer to delete.</param>
        /// <returns>A task representing the asynchronous operation. True if the manufacturer is deleted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown when the manufacturer does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when failed to delete the manufacturer.</exception>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingManufacturer = await _manufacturerRepository.GetByIdAsync(id);

            if (existingManufacturer is null)
            {
                throw new ArgumentException("Manufacturer does not exist");
            }

            if (!await _manufacturerRepository.DeleteAsync(existingManufacturer))
            {
                throw new InvalidOperationException("Failed to delete manufacturer");
            }

            return true;
        }
    }
}
