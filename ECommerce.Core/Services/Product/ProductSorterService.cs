using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Product
{
    public class ProductSorterService : IProductSorterService
    {
        public ProductSorterService(IProductRepository productRepository)
        {
            
        }

        public Task<IEnumerable<ProductDto>> SortAsync(IEnumerable<ProductDto> products, string? sortBy, SortOrder sortOrder = SortOrder.ASC, bool onSaleFirst = false)
        {
            throw new NotImplementedException();
        }
    }
}
