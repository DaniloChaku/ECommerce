using ECommerce.Core.Domain.RepositoryContracts;
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
        public CategoryDeleterService(ICategoryRepository categoryRepository)
        {

        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
