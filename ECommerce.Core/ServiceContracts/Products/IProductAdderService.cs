using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Products
{
    /// <summary>
    /// Defines the contract for adding a product.
    /// </summary>
    public interface IProductAdderService
    {
        /// <summary>
        /// Adds a new product asynchronously.
        /// </summary>
        /// <param name="productDto">The data transfer object representing the product to add.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the added product.</returns>
        Task<ProductDto> AddAsync(ProductDto productDto);
    }
}
