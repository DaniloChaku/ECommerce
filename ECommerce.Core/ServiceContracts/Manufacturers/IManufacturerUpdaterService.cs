using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    public interface IManufacturerUpdaterService
    {
        Task<ManufacturerDto> UpdateAsync(ManufacturerDto manufacturerDto);
    }
}
