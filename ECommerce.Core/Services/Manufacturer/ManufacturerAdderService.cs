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
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerAdderService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<ManufacturerDto> AddAsync(ManufacturerDto manufacturerDto)
        {
            if (manufacturerDto is null)
            {
                throw new ArgumentNullException(nameof(manufacturerDto), "manufacturer data cannot be null");
            }

            if (string.IsNullOrEmpty(manufacturerDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(manufacturerDto.Name));
            }

            if (manufacturerDto.Id != Guid.Empty)
            {
                throw new ArgumentException("Id must be empty", nameof(manufacturerDto.Id));
            }

            var existingManufacturers = await _manufacturerRepository.GetAllAsync(t => t.Name == manufacturerDto.Name);
            if (existingManufacturers.Any())
            {
                throw new ArgumentException("manufacturer with the same name already exists");
            }

            var manufacturer = manufacturerDto.ToEntity();

            var manufacturerAdded = await _manufacturerRepository.AddAsync(manufacturer);

            return manufacturerAdded.ToDto();
        }
    }
}
