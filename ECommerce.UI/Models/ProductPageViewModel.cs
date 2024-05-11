using ECommerce.Core.Dtos;

namespace ECommerce.UI.Models
{
    /// <summary>
    /// Represents the view model used to display a page of products.
    /// </summary>
    public class ProductPageViewModel
    {
        /// <summary>
        /// Gets or sets the list of products to display on the page.
        /// </summary>
        public List<ProductDto> Products { get; set; } = [];

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the starting page number of the pagination range.
        /// </summary>
        public int PaginationStart { get; set; }

        /// <summary>
        /// Gets or sets the ending page number of the pagination range.
        /// </summary>
        public int PaginationEnd { get; set; }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the search query used to filter products.
        /// </summary>
        public string? SearchQuery { get; set; }
    }
}
