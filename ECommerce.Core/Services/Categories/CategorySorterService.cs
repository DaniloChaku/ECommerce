using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Categories;

namespace ECommerce.Core.Services.Categories
{
    public class CategorySorterService : ICategorySorterService
    {
        public List<CategoryDto> Sort(IEnumerable<CategoryDto> categories,
            SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.ASC)
            {
                return categories.OrderBy(c => c.Name).ToList();
            }

            return categories.OrderByDescending(c => c.Name).ToList();
        }
    }
}
