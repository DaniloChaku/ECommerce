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
    public class ManufacturerAdderService : IManufacturerAdderService
    {
        public ManufacturerAdderService(IManufacturerRepository manufacturerRepository)
        {
            
        }

        public Task<ManufacturerDto> AddAsync(ManufacturerDto manufacturerDto)
        {
            throw new NotImplementedException();
        }
    }
}
