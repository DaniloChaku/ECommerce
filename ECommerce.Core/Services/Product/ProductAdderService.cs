using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Helpers;
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

            if (string.IsNullOrEmpty(productDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(productDto.Name));
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

            ValidationHelper.ValidateModel(product);

            var productAdded = await _productRepository.AddAsync(product);

            return productAdded.ToDto();
        }
    }
}
