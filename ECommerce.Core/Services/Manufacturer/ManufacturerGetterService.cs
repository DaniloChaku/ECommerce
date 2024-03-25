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
    public class ManufacturerGetterService : IManufacturerGetterService
    {
        public ManufacturerGetterService(IManufacturerRepository manufacturerRepository)
        {
            
        }

        public Task<IEnumerable<ManufacturerDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ManufacturerDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
