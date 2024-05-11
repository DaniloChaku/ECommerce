using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    /// <summary>
    /// Service for sorting manufacturer DTOs.
    /// </summary>
    public class ManufacturerSorterService : IManufacturerSorterService
    {
        /// <summary>
        /// Sorts a collection of manufacturer DTOs based on the manufacturer name.
        /// </summary>
        /// <param name="manufacturers">The collection of manufacturer DTOs to sort.</param>
        /// <param name="sortOrder">The sort order (ascending or descending).</param>
        /// <returns>The sorted list of manufacturer DTOs.</returns>
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
