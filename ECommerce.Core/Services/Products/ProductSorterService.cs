using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Products;

namespace ECommerce.Core.Services.Products
{
    /// <summary>
    /// Service for sorting lists of products based on various criteria.
    /// </summary>
    public class ProductSorterService : IProductSorterService
    {
        /// <summary>
        /// Sorts a collection of product DTOs based on the specified criteria.
        /// </summary>
        /// <param name="products">The collection of product DTOs to be sorted.</param>
        /// <param name="sortBy">The property to sort by (e.g., "Name", "Price").</param>
        /// <param name="sortOrder">The sort order (ASC or DESC).</param>
        /// <param name="onSaleFirst">Specifies whether products on sale should be sorted first.</param>
        /// <returns>The sorted list of product DTOs.</returns>
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
