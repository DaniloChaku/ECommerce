using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Categories
{
    /// <summary>
    /// Defines the contract for updating a category.
    /// </summary>
    public interface ICategoryUpdaterService
    {
        /// <summary>
        /// Updates a category asynchronously.
        /// </summary>
        /// <param name="categoryDto">The data transfer object representing the category to update.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the updated category.</returns>
        Task<CategoryDto> UpdateAsync(CategoryDto categoryDto);
    }
}
