using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;

namespace ECommerce.Infrastructure.Repositories
{
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(ApplicationDbContext context) : base(context)
        {
        }

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
