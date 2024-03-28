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
        public List<ProductDto> Sort(IEnumerable<ProductDto> products, 
            string? sortBy, SortOrder sortOrder = SortOrder.ASC, bool onSaleFirst = false)
        {
            if (string.IsNullOrEmpty(sortBy)) return products.ToList();

            var sortedProducts = (sortBy, sortOrder) switch
            {
                (nameof(ProductDto.Name), SortOrder.ASC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenBy(temp => temp.Name, StringComparer.OrdinalIgnoreCase) :
                products.OrderBy(temp => temp.Name, StringComparer.OrdinalIgnoreCase),

                (nameof(ProductDto.Name), SortOrder.DESC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenByDescending(temp => temp.Name, StringComparer.OrdinalIgnoreCase) :
                products.OrderByDescending(temp => temp.Name, StringComparer.OrdinalIgnoreCase),

                (nameof(ProductDto.Price), SortOrder.ASC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenBy(temp => temp.Price) :
                products.OrderBy(temp => temp.Price),

                (nameof(ProductDto.Price), SortOrder.DESC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenByDescending(temp => temp.Price) :
                products.OrderByDescending(temp => temp.Price),

                (nameof(ProductDto.SalePrice), SortOrder.ASC)
                => products.OrderBy(temp => temp.SalePrice),

                (nameof(ProductDto.SalePrice), SortOrder.DESC)
                => products.OrderByDescending(temp => temp.SalePrice),

                (nameof(ProductDto.ManufacturerName), SortOrder.ASC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenBy(temp => temp.ManufacturerName, StringComparer.OrdinalIgnoreCase) :
                products.OrderBy(temp => temp.ManufacturerName, StringComparer.OrdinalIgnoreCase),

                (nameof(ProductDto.ManufacturerName), SortOrder.DESC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenByDescending(temp => temp.ManufacturerName, StringComparer.OrdinalIgnoreCase) :
                products.OrderByDescending(temp => temp.ManufacturerName, StringComparer.OrdinalIgnoreCase),

                (nameof(ProductDto.CategoryName), SortOrder.ASC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenBy(temp => temp.CategoryName, StringComparer.OrdinalIgnoreCase) :
                products.OrderBy(temp => temp.CategoryName, StringComparer.OrdinalIgnoreCase),

                (nameof(ProductDto.CategoryName), SortOrder.DESC)
                => onSaleFirst ? products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1)
                .ThenByDescending(temp => temp.CategoryName, StringComparer.OrdinalIgnoreCase) :
                products.OrderByDescending(temp => temp.CategoryName, StringComparer.OrdinalIgnoreCase),

                _ => products
            };

            return sortedProducts.ToList();
        }
    }
}
