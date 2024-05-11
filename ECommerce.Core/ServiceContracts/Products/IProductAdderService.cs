using ECommerce.Core.Dtos;

namespace ECommerce.Core.ServiceContracts.Products
{
    public interface IProductAdderService
    {
        Task<ProductDto> AddAsync(ProductDto productDto);
    }
}
