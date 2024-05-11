using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;

namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategorySorterService
    {
        List<CategoryDto> Sort(IEnumerable<CategoryDto> categories,
            SortOrder sortOrder = SortOrder.ASC);
    }
}
