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
    public class ProductAdderService : IProductAdderService
    {
        private readonly IProductRepository _productRepository;

        public ProductAdderService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            if (productDto is null)
            {
                throw new ArgumentNullException(nameof(productDto), "Product data cannot be null");
            }

            if (productDto.Id != Guid.Empty)
            {
                throw new ArgumentException("Id must be empty", nameof(productDto.Id));
            }

            var existingProducts = await _productRepository.GetAllAsync(t => t.Name == productDto.Name);
            if (existingProducts.Any())
            {
                throw new ArgumentException("Product with the same name already exists");
            }

            var product = productDto.ToEntity();

            var productAdded = await _productRepository.AddAsync(product);

            return productAdded.ToDto();
        }
    }
}
