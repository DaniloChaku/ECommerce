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
    public class CategoryAdderService : ICategoryAdderService
    {
        public CategoryAdderService(ICategoryRepository categoryRepository)
        {

        }

        public Task<CategoryDto> AddAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
