using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Products
{
    /// <summary>
    /// Defines the contract for updating a product.
    /// </summary>
    public interface IProductUpdaterService
    {
        /// <summary>
        /// Updates a product asynchronously.
        /// </summary>
        /// <param name="productDto">The data transfer object representing the product to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated product.</returns>
        Task<ProductDto> UpdateAsync(ProductDto productDto);
    }
}
