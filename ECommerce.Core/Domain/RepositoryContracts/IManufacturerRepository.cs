using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository interface for managing manufacturers.
    /// </summary>
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        /// <summary>
        /// Updates a manufacturer asynchronously in the database.
        /// </summary>
        /// <param name="manufacturer">The manufacturer to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated manufacturer.</returns>
        Task<Manufacturer> UpdateAsync(Manufacturer manufacturer);
    }
}
