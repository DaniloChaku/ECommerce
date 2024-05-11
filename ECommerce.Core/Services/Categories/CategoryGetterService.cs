using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    public class CategoryGetterService : ICategoryGetterService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryGetterService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => c.ToDto()).ToList();
        }

        public async Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            return category?.ToDto();
        }
    }
}
