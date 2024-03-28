using ECommerce.Core.Domain.Entities;
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
        private readonly IProductRepository _productRepository;

        public ProductGetterService(IProductRepository productRepository)
        {
            _productRepository = productRepository; 
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            return product?.ToDto();
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(t => t.ToDto()).ToList();
        }

        public async Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetAllAsync(t => t.CategoryId == categoryId);

            return products.Select(t => t.ToDto()).ToList();
        }

        public async Task<List<ProductDto>> GetByManufacturerAsync(Guid manufacturerId)
        {
            var products = await _productRepository.GetAllAsync(t => t.ManufacturerId == manufacturerId);

            return products.Select(t => t.ToDto()).ToList();
        }
    }
}
