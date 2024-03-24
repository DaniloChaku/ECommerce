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
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId);
        Task<IEnumerable<ProductDto>> GetByManufacturerAsync(Guid manufacturerId);
        Task<IEnumerable<ProductDto>> GetByNameAsync(string name);
    }
}
