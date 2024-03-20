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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context) : base(context)
        {
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            var existingCategory = await _dbSet.FindAsync(category.Id);
            if (existingCategory == null)
            {
                return false;
            }

            existingCategory.Name = category.Name;

            return await SaveAsync();
        }
    }
}
