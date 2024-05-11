using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;

namespace ECommerce.Core.ServiceContracts.Products
{
    public interface IProductSorterService
    {
        List<ProductDto> Sort(IEnumerable<ProductDto> products,
            string? sortBy, SortOrder sortOrder = SortOrder.ASC, bool onSaleFirst = false);
    }
}
