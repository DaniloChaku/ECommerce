using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;

namespace ECommerce.Core.ServiceContracts.Categories
{
    /// <summary>
    /// Defines the contract for sorting categories.
    /// </summary>
    public interface ICategorySorterService
    {
        /// <summary>
        /// Sorts a collection of categories.
        /// </summary>
        /// <param name="categories">The collection of category DTOs to sort.</param>
        /// <param name="sortOrder">The sort order to apply. Default is ascending.</param>
        /// <returns>A sorted list of category DTOs.</returns>
        List<CategoryDto> Sort(IEnumerable<CategoryDto> categories, SortOrder sortOrder = SortOrder.ASC);
    }
}
