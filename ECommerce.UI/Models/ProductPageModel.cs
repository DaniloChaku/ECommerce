using ECommerce.Core.Dtos;

namespace ECommerce.UI.Models
{
    public class ProductPageModel
    {
        public List<ProductDto> Products { get; set; } = [];
        public int CurrentPage { get; set; }
        public int PaginationStart { get; set; }  
        public int PaginationEnd { get; set; }
        public int TotalPages { get; set; }
        public string? SearchQuery { get; set; }
    }
}
