using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Core.Services.Products
{
    /// <summary>
    /// Service for retrieving products.
    /// </summary>
    public class ProductGetterService : IProductGetterService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductGetterService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for interacting with products.</param>
        public ProductGetterService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>The product with the specified ID, or null if not found.</returns>
        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            return product?.ToDto();
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return ConvertToProductDtos(products);
        }

        /// <summary>
        /// Retrieves products by category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A list of products belonging to the specified category.</returns>
        public async Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetAllAsync(p => p.CategoryId == categoryId);

            return ConvertToProductDtos(products);
        }

        /// <summary>
        /// Retrieves products by manufacturer.
        /// </summary>
        /// <param name="manufacturerId">The ID of the manufacturer.</param>
        /// <returns>A list of products manufactured by the specified manufacturer.</returns>
        public async Task<List<ProductDto>> GetByManufacturerAsync(Guid manufacturerId)
        {
            var products = await _productRepository.GetAllAsync(p => p.ManufacturerId == manufacturerId);

            return ConvertToProductDtos(products);
        }

        /// <summary>
        /// Retrieves products based on a search query.
        /// </summary>
        /// <param name="searchQuery">The search query string.</param>
        /// <returns>A list of products matching the search query.</returns>
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
