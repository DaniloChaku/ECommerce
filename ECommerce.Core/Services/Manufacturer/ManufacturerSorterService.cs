using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Manufacturer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Manufacturer
{
    public class ManufacturerSorterService : IManufacturerSorterService
    {
        public List<ManufacturerDto> Sort(IEnumerable<ManufacturerDto> manufacturers,
            SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.ASC)
            {
                return manufacturers.OrderBy(t => t.Name).ToList();
            }

            return manufacturers.OrderByDescending(t => t.Name).ToList();
        }
    }
}
