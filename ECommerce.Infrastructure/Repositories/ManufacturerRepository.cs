using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(DbContext context) : base(context)
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
