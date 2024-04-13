using ECommerce.Core.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.UI.Models
{
    public class ProductUpsertModel
    {
        public ProductDto Product { get; set; } = new ProductDto();
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Manufacturers { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
