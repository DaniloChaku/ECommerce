using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Manufacturer
{
    public interface IManufacturerSorterService
    {
        Task<IEnumerable<ManufacturerDto>> SortAsync(IEnumerable<ManufacturerDto> manufacturers, SortOrder sortOrder);
    }
}
