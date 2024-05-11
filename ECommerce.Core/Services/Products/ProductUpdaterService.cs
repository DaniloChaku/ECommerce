using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Products;

namespace ECommerce.Core.Services.Products
{
    /// <summary>
    /// Service for updating products.
    /// </summary>
    public class ProductUpdaterService : IProductUpdaterService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUpdaterService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for interacting with products.</param>
        public ProductUpdaterService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Updates an existing product with the provided data.
        /// </summary>
        /// <param name="productDto">The data for the product to be updated.</param>
        /// <returns>The updated product.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided product data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided product ID is empty or when the product does not exist.</exception>
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
