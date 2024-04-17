using ECommerce.Core.DTO;
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
        [DataType(DataType.Upload)]
        public IFormFile? Image { get; set; }
        public ImageUploadOptions? ImageUploadOptions { get; set; }
    }
}
