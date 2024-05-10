using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Products
{
    public interface IProductGetterService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId);
        Task<List<ProductDto>> GetByManufacturerAsync(Guid manufacturerId);
        Task<List<ProductDto>> GetBySearchQueryAsync(string? searchQuery);
    }
}
