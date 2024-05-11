using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Products;

namespace ECommerce.Core.Services.Products
{
    /// <summary>
    /// Service for adding new products.
    /// </summary>
    public class ProductAdderService : IProductAdderService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductAdderService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for interacting with products.</param>
        public ProductAdderService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDto">The DTO containing the product information to be added.</param>
        /// <returns>The added product DTO.</returns>
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
