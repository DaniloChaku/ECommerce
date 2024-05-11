using ECommerce.Core.Dtos;
using ECommerce.Core.Helpers.ValidationAttributes;
using ECommerce.Core.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.UI.Models
{
    /// <summary>
    /// Represents the view model used for creating or updating a product.
    /// </summary>
    public class ProductUpsertViewModel
    {
        /// <summary>
        /// Gets or sets the product DTO for upsert operation.
        /// </summary>
        public ProductDto Product { get; set; } = new ProductDto();

        /// <summary>
        /// Gets or sets the list of categories for the product.
        /// </summary>
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();

        /// <summary>
        /// Gets or sets the list of manufacturers for the product.
        /// </summary>
        public IEnumerable<SelectListItem> Manufacturers { get; set; } = Enumerable.Empty<SelectListItem>();

        /// <summary>
        /// Gets or sets the list of price types for the product.
        /// </summary>
        public IEnumerable<SelectListItem> PriceTypes { get; set; } = Enumerable.Empty<SelectListItem>();

        /// <summary>
        /// Gets or sets the uploaded image file for the product.
        /// </summary>
        [DataType(DataType.Upload)]
        [ImageSize(1024, 1024)]
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Gets or sets the options for uploading images.
        /// </summary>
        public ImageUploadOptions? ImageUploadOptions { get; set; }
    }
}
