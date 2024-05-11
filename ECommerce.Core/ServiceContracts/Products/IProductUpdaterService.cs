using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Products
{
    public interface IProductUpdaterService
    {
        Task<ProductDto> UpdateAsync(ProductDto productDto);
    }
}
