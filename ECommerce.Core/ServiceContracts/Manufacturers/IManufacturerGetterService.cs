using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Manufacturers
{
    public interface IManufacturerGetterService
    {
        Task<List<ManufacturerDto>> GetAllAsync();
        Task<ManufacturerDto?> GetByIdAsync(Guid id);
    }
}
