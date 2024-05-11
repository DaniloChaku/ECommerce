using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;

namespace ECommerce.Core.ServiceContracts.Products
{
    /// <summary>
    /// Defines the contract for sorting products.
    /// </summary>
    public interface IProductSorterService
    {
        /// <summary>
        /// Sorts a collection of products based on specified criteria.
        /// </summary>
        /// <param name="products">The collection of product DTOs to sort.</param>
        /// <param name="sortBy">The property to sort by. Can be null for default sorting.</param>
        /// <param name="sortOrder">The sort order to apply. Default is ascending.</param>
        /// <param name="onSaleFirst">Indicates whether to sort products on sale first.</param>
        /// <returns>A sorted list of product DTOs.</returns>
        List<ProductDto> Sort(IEnumerable<ProductDto> products, string? sortBy, SortOrder sortOrder = SortOrder.ASC, bool onSaleFirst = false);
    }
}
