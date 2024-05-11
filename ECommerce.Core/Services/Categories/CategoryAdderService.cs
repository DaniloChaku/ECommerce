using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    /// <summary>
    /// Service for adding categories.
    /// </summary>
    public class CategoryAdderService : ICategoryAdderService
    {
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAdderService"/> class.
        /// </summary>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryAdderService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Adds a category asynchronously.
        /// </summary>
        /// <param name="categoryDto">The data transfer object representing the category to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added category.</returns>
        /// <exception cref="ArgumentNullException">Thrown when categoryDto is null.</exception>
        /// <exception cref="ArgumentException">Thrown when categoryDto.Id is not empty or 
        /// when a category with the same name already exists.</exception>
        public async Task<CategoryDto> AddAsync(CategoryDto categoryDto)
        {
            if (categoryDto is null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null");
            }

            if (categoryDto.Id != Guid.Empty)
            {
                throw new ArgumentException("Id must be empty", nameof(categoryDto.Id));
            }

            var existingCategories = await _categoryRepository.GetAllAsync(t => t.Name == categoryDto.Name);
            if (existingCategories.Any())
            {
                throw new ArgumentException("Category with the same name already exists");
            }

            var category = categoryDto.ToEntity();

            var categoryAdded = await _categoryRepository.AddAsync(category);

            return categoryAdded.ToDto();
        }
    }
}
