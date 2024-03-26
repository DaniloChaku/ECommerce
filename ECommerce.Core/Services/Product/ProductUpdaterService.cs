using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Product
{
    public class ProductUpdaterService : IProductUpdaterService
    {
        public ProductUpdaterService(IProductRepository productRepository)
        {
            
        }

        public Task<bool> UpdateAsync(ProductDto product)
        {
            throw new NotImplementedException();
        }
    }
}
