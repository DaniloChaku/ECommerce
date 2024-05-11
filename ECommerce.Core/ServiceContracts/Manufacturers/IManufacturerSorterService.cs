using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    /// <summary>
    /// Defines the contract for sorting manufacturers.
    /// </summary>
    public interface IManufacturerSorterService
    {
        /// <summary>
        /// Sorts a collection of manufacturers.
        /// </summary>
        /// <param name="manufacturers">The collection of manufacturer DTOs to sort.</param>
        /// <param name="sortOrder">The sort order to apply. Default is ascending.</param>
        /// <returns>A sorted list of manufacturer DTOs.</returns>
        List<ManufacturerDto> Sort(IEnumerable<ManufacturerDto> manufacturers, SortOrder sortOrder = SortOrder.ASC);
    }
}
