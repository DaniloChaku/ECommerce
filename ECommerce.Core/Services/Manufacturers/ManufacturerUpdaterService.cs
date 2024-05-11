﻿using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Manufacturers;

namespace ECommerce.Core.Services.Manufacturers
{
    public class ManufacturerUpdaterService : IManufacturerUpdaterService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerUpdaterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<ManufacturerDto> UpdateAsync(ManufacturerDto manufacturerDto)
        {
            if (manufacturerDto is null)
            {
                throw new ArgumentNullException(nameof(manufacturerDto), "Manufacturer data cannot be null");
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

            var manufacturerUpdated = await _manufacturerRepository.UpdateAsync(manufacturer);

            return manufacturerUpdated.ToDto();
        }
    }
}
