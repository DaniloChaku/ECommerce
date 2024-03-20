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

        public async Task<bool> UpdateAsync(Manufacturer manufacturer)
        {
            var existingManufacturer = await _dbSet.FindAsync(manufacturer.Id);
            if (existingManufacturer == null)
            {
                return false;
            }

            existingManufacturer.Name = manufacturer.Name;

            return await SaveAsync();
        }
    }
}
