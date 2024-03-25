using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.ServiceContracts.Manufacturer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Manufacturer
{
    public class ManufacturerDeleterService : IManufacturerDeleterService
    {
        public ManufacturerDeleterService(IManufacturerRepository manufacturerRepository)
        {
            
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
