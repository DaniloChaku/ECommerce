using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Categories
{
    /// <summary>
    /// Defines the contract for retrieving categories.
    /// </summary>
    public interface ICategoryGetterService
    {
        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains a list of categories.</returns>
        Task<List<CategoryDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a category by its identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the category if found; otherwise, null.
        /// </returns>
        Task<CategoryDto?> GetByIdAsync(Guid id);
    }
}
