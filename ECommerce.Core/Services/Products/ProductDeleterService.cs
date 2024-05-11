using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Products;

namespace ECommerce.Core.Services.Products
{
    public class ProductDeleterService : IProductDeleterService
    {
        private readonly IProductRepository _productRepository;

        public ProductDeleterService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

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
