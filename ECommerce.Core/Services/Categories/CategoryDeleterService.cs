using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    /// <summary>
    /// Service for deleting categories.
    /// </summary>
    public class CategoryDeleterService : ICategoryDeleterService
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDeleterService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryDeleterService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Deletes a category asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the category to delete.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result indicates whether the deletion was successful.</returns>
        /// <exception cref="ArgumentException">Thrown when the category with the specified id does not exist.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the deletion operation fails.</exception>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);

            if (existingCategory is null)
            {
                throw new ArgumentException("Category does not exist");
            }

            if (!await _categoryRepository.DeleteAsync(existingCategory))
            {
                throw new InvalidOperationException("Failed to delete category");
            }

            return true;
        }
    }
}
