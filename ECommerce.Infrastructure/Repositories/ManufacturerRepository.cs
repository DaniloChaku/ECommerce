using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;

namespace ECommerce.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing manufacturers.
    /// </summary>
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManufacturerRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public ManufacturerRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Updates an existing manufacturer.
        /// </summary>
        /// <param name="manufacturer">The updated manufacturer data.</param>
        /// <returns>The updated manufacturer.</returns>
        public async Task<Manufacturer> UpdateAsync(Manufacturer manufacturer)
        {
            var existingManufacturer = await _dbSet.FindAsync(manufacturer.Id);

            existingManufacturer!.Name = manufacturer.Name;
            existingManufacturer.Description = manufacturer.Description;

            await _context.SaveChangesAsync();

            return existingManufacturer;
        }
    }
}
