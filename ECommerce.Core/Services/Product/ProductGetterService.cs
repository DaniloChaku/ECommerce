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
    public class ProductGetterService : IProductGetterService
    {
        public ProductGetterService(IProductRepository productRepository)
        {
            
        }

        public Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetByManufacturerAsync(Guid manufacturerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
