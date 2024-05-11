using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    /// <summary>
    /// Service for sorting categories.
    /// </summary>
    public class CategorySorterService : ICategorySorterService
    {
        /// <summary>
        /// Sorts categories based on the provided sort order asynchronously.
        /// </summary>
        /// <param name="categories">The collection of categories to sort.</param>
        /// <param name="sortOrder">The sort order (ASC or DESC).</param>
        /// <returns>A list of sorted category DTOs.</returns>
        public List<CategoryDto> Sort(IEnumerable<CategoryDto> categories, SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.ASC)
            {
                return categories.OrderBy(c => c.Name).ToList();
            }

            return categories.OrderByDescending(c => c.Name).ToList();
        }
    }
}
