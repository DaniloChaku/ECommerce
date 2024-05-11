using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    public class CategoryUpdaterService : ICategoryUpdaterService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryUpdaterService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> UpdateAsync(CategoryDto categoryDto)
        {
            if (categoryDto is null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null");
            }

            if (categoryDto.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(categoryDto.Id));
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (existingCategory is null)
            {
                throw new ArgumentException("Category does not exist");
            }

            var category = categoryDto.ToEntity();

            var categoryUpdated = await _categoryRepository.UpdateAsync(category);

            return categoryUpdated.ToDto();
        }
    }
}
