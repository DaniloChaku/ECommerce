using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    public class ManufacturerSorterService : IManufacturerSorterService
    {
        public List<ManufacturerDto> Sort(IEnumerable<ManufacturerDto> manufacturers,
            SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.ASC)
            {
                return manufacturers.OrderBy(m => m.Name).ToList();
            }

            return manufacturers.OrderByDescending(m => m.Name).ToList();
        }
    }
}
