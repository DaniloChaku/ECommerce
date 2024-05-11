using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    public interface IManufacturerAdderService
    {
        Task<ManufacturerDto> AddAsync(ManufacturerDto manufacturerDto);
    }
}
