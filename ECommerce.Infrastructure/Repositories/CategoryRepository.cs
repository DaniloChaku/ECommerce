using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await _dbSet.FirstOrDefaultAsync(t => t.Id == category.Id);

            existingCategory!.Name = category.Name;

            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
