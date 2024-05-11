using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Categories
{
    /// <summary>
    /// Defines the contract for adding a category.
    /// </summary>
    public interface ICategoryAdderService
    {
        /// <summary>
        /// Adds a new category asynchronously.
        /// </summary>
        /// <param name="categoryDto">The data transfer object representing the category to add.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added category.</returns>
        Task<CategoryDto> AddAsync(CategoryDto categoryDto);
    }
}
