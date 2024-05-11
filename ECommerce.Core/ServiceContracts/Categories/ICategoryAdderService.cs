using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategoryAdderService
    {
        Task<CategoryDto> AddAsync(CategoryDto categoryDto);
    }
}
