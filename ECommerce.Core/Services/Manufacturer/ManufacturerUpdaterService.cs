using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Manufacturer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Manufacturer
{
    public class ManufacturerUpdaterService : IManufacturerUpdaterService
    {
        public ManufacturerUpdaterService(IManufacturerRepository manufacturerRepository)
        {
            
        }

        public Task<bool> UpdateAsync(ManufacturerDto manufacturerDto)
        {
            throw new NotImplementedException();
        }
    }
}
