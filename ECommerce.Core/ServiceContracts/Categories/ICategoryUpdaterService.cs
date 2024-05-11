using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Categories
{
    public interface ICategoryUpdaterService
    {
        Task<CategoryDto> UpdateAsync(CategoryDto categoryDto);
    }
}
