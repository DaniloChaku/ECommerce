using ECommerce.Core.Domain.Entities;

namespace ECommerce.Core.Domain.RepositoryContracts
{
    /// <summary>
    /// Represents a repository interface for managing categories.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Updates a category asynchronously in the database.
        /// </summary>
        /// <param name="category">The category to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated category.</returns>
        Task<Category> UpdateAsync(Category category);
    }
}
