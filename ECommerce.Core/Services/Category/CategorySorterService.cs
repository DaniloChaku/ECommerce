using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Category
{
    public class CategorySorterService : ICategorySorterService
    {
        public CategorySorterService(ICategoryRepository categoryRepository)
        {

        }

        public Task<IEnumerable<CategoryDto>> SortAsync(IEnumerable<CategoryDto> categories, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }
    }
}
