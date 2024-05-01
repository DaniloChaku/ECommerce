using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Category
{
    public class CategoryDeleterService : ICategoryDeleterService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDeleterService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

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
