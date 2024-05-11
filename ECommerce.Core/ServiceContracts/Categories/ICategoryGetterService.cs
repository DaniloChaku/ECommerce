using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategoryGetterService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(Guid id);
    }
}
