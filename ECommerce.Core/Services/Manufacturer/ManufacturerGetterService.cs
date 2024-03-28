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
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerGetterService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<List<ManufacturerDto>> GetAllAsync()
        {
            var categories = await _manufacturerRepository.GetAllAsync();

            return categories.Select(t => t.ToDto()).ToList();
        }

        public async Task<ManufacturerDto?> GetByIdAsync(Guid id)
        {
            var manufacturer = await _manufacturerRepository.GetByIdAsync(id);

            return manufacturer?.ToDto();
        }
    }
}
