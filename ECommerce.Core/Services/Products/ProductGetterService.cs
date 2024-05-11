using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Core.Services.Products
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

            return ConvertToProductDtos(products);
        }

        public async Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetAllAsync(p => p.CategoryId == categoryId);

            return ConvertToProductDtos(products);
        }

        public async Task<List<ProductDto>> GetByManufacturerAsync(Guid manufacturerId)
        {
            var products = await _productRepository.GetAllAsync(p => p.ManufacturerId == manufacturerId);

            return ConvertToProductDtos(products);
        }

        public async Task<List<ProductDto>> GetBySearchQueryAsync(string? searchQuery)
        {
            var searchQueryTrimmed = searchQuery?.Trim().ToLower();
            List<Product> products;

            if (string.IsNullOrWhiteSpace(searchQueryTrimmed))
            {
                products = await _productRepository.GetAllAsync();
            }
            else
            {
                products = await _productRepository.GetAllAsync(p =>
                EF.Functions.Like(p.Name, $"%{searchQueryTrimmed}%"));
            }

            return ConvertToProductDtos(products);
        }

        private List<ProductDto> ConvertToProductDtos(IEnumerable<Product> products)
        {
            return products.Select(p => p.ToDto()).ToList();
        }
    }
}
