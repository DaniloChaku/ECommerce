using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Products
{
    /// <summary>
    /// Defines the contract for retrieving products.
    /// </summary>
    public interface IProductGetterService
    {
        /// <summary>
        /// Retrieves all products asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of products.</returns>
        Task<List<ProductDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a product by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the product if found; otherwise, null.
        /// </returns>
        Task<ProductDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves products by category asynchronously.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of products.</returns>
        Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId);

        /// <summary>
        /// Retrieves products by manufacturer asynchronously.
        /// </summary>
        /// <param name="manufacturerId">The unique identifier of the manufacturer.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of products.</returns>
        Task<List<ProductDto>> GetByManufacturerAsync(Guid manufacturerId);

        /// <summary>
        /// Retrieves products by a search query asynchronously.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of products.</returns>
        Task<List<ProductDto>> GetBySearchQueryAsync(string? searchQuery);
    }
}
