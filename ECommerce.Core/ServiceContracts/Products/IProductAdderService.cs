using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Products
{
    public interface IProductAdderService
    {
        Task<ProductDto> AddAsync(ProductDto productDto);
    }
}
