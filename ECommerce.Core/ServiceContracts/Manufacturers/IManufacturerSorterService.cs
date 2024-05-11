using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    public interface IManufacturerSorterService
    {
        List<ManufacturerDto> Sort(
            IEnumerable<ManufacturerDto> manufacturers, SortOrder sortOrder = SortOrder.ASC);
    }
}
