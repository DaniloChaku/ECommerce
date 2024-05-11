using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Products;

namespace ECommerce.Core.Services.Products
{
    /// <summary>
    /// Service for deleting products.
    /// </summary>
    public class ProductDeleterService : IProductDeleterService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDeleterService"/> class.
        /// </summary>
        /// <param name="productRepository">The repository for interacting with products.</param>
        public ProductDeleterService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);

            if (existingProduct is null)
            {
                throw new ArgumentException("Product does not exist");
            }

            if (!await _productRepository.DeleteAsync(existingProduct))
            {
                throw new InvalidOperationException("Failed to delete product");
            }

            return true;
        }
    }
}
