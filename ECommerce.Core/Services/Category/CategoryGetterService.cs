using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Category
{
    public class CategoryGetterService : ICategoryGetterService
    {
        public CategoryGetterService(ICategoryRepository categoryRepository)
        {

        }

        public Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
