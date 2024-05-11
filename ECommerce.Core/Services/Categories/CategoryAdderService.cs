using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    public class CategoryAdderService : ICategoryAdderService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAdderService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

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
