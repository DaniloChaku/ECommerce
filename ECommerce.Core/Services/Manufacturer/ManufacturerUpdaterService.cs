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
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerUpdaterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> UpdateAsync(ManufacturerDto manufacturerDto)
        {
            if (manufacturerDto is null)
            {
                throw new ArgumentNullException(nameof(manufacturerDto), "Manufacturer data cannot be null");
            }

            if (string.IsNullOrEmpty(manufacturerDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(manufacturerDto.Name));
            }

            if (manufacturerDto.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(manufacturerDto.Id));
            }

            var existingManufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerDto.Id);
            if (existingManufacturer is null)
            {
                throw new ArgumentException("Manufacturer does not exist");
            }

            var manufacturer = manufacturerDto.ToEntity();

            if (!await _manufacturerRepository.UpdateAsync(manufacturer))
            {
                throw new InvalidOperationException("Failed to update manufacturer");
            }

            return true;
        }
    }
}
