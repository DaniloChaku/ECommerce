using ECommerce.Core.Dtos;
using ECommerce.Core.Helpers.ValidationAttributes;
using ECommerce.Core.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.UI.Models
{
    public class ProductUpsertModel
    {
        public ProductDto Product { get; set; } = new ProductDto();
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Manufacturers { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> PriceTypes { get; set; } = Enumerable.Empty<SelectListItem>();
        [DataType(DataType.Upload)]
        [ImageSize(1024, 1024)]
        public IFormFile? Image { get; set; }
        public ImageUploadOptions? ImageUploadOptions { get; set; }
    }
}
