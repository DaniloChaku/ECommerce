using ECommerce.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Product
{
    public interface IProductGetterService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<List<ProductDto>> GetByCategoryAsync(Guid categoryId);
        Task<List<ProductDto>> GetByManufacturerAsync(Guid manufacturerId);
    }
}
