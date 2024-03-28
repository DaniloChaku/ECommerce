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
    public class ManufacturerDeleterService : IManufacturerDeleterService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerDeleterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingManufacturer = await _manufacturerRepository.GetByIdAsync(id);

            if (existingManufacturer is null)
            {
                throw new ArgumentException("Manufacturer does not exist");
            }

            if (!await _manufacturerRepository.DeleteAsync(existingManufacturer))
            {
                throw new InvalidOperationException("Failed to delete manufacturer");
            }

            return true;
        }
    }
}
