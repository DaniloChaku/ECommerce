using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Helpers;
using ECommerce.Core.ServiceContracts.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Products
{
    public class ProductUpdaterService : IProductUpdaterService
    {
        private readonly IProductRepository _productRepository;

        public ProductUpdaterService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> UpdateAsync(ProductDto productDto)
        {
            if (productDto is null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null");
            }

            if (productDto.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(productDto.Id));
            }

            var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
            if (existingProduct is null)
            {
                throw new ArgumentException("Product does not exist");
            }

            var product = productDto.ToEntity();

            var productUpdated = await _productRepository.UpdateAsync(product);

            return productUpdated.ToDto();
        }
    }
}
