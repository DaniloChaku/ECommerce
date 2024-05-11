using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing categories.
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The updated category data.</param>
        /// <returns>The updated category.</returns>
        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await _dbSet.FirstOrDefaultAsync(t => t.Id == category.Id);

            existingCategory!.Name = category.Name;

            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
