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
        public ManufacturerSorterService(IManufacturerRepository manufacturerRepository)
        {
            
        }

        public Task<IEnumerable<ManufacturerDto>> SortAsync(
            IEnumerable<ManufacturerDto> manufacturers, SortOrder sortOrder = SortOrder.ASC)
        {
            throw new NotImplementedException();
        }
    }
}
