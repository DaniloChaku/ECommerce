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
    public class ProductAdderService : IProductAdderService
    {
        public ProductAdderService(IProductRepository productRepository)
        {
            
        }

        public Task<ProductDto> AddAsync(ProductDto productDto)
        {
            throw new NotImplementedException();
        }
    }
}
