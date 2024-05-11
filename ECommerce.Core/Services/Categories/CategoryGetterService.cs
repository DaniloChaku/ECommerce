using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    /// <summary>
    /// Service for retrieving categories.
    /// </summary>
    public class CategoryGetterService : ICategoryGetterService
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryGetterService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryGetterService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of category DTOs.</returns>
        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => c.ToDto()).ToList();
        }

        /// <summary>
        /// Retrieves a category by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the category to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the category DTO if found; otherwise, null.</returns>
        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            return category?.ToDto();
        }
    }
}
