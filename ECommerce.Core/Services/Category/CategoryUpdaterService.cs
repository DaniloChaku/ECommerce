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
    public class CategoryUpdaterService : ICategoryUpdaterService
    {
        public CategoryUpdaterService(ICategoryRepository categoryRepository)
        {

        }

        public Task<bool> UpdateAsync(CategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
