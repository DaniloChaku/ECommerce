using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Enums;
using ECommerce.Core.Helpers;
using ECommerce.Core.Helpers.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    /// <summary>
    /// Represents a product data transfer object (DTO).
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [RegularExpression(@"^\S(.*\S)?$",
            ErrorMessage = "The {0} field must not start or end with whitespace characters and must contain at least one letter.")]
        [Remote(controller: "Products", action: "IsProductNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Product with the same name already exists")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [MaxLength(400)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Display(Name = "Price")]
        [Range(typeof(decimal), "0.01", Constants.MAX_DECIMAL_VALUE_STRING,
            ParseLimitsInInvariantCulture = true,
            ErrorMessage = "{0} must be greater than 0")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the sale price of the product.
        /// </summary>
        [Display(Name = "Sale Price")]
        [Range(typeof(decimal), "0.01", Constants.MAX_DECIMAL_VALUE_STRING,
            ParseLimitsInInvariantCulture = true,
            ErrorMessage = "{0} must be greater than 0")]
        [LessThan(nameof(Price),
            ErrorMessage = "{0} must be less than {1}")]
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// Gets or sets the price type of the product.
        /// </summary>
        public PriceType PriceType { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity of the product.
        /// </summary>
        [Required]
        [Range(0, long.MaxValue,
            ErrorMessage = "The number of products in stock must not be negative")]
        public long Stock { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the manufacturer associated with the product.
        /// </summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer associated with the product.
        /// </summary>
        public string? ManufacturerName { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the category associated with the product.
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category associated with the product.
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Converts the product DTO to its corresponding entity.
        /// </summary>
        /// <returns>The entity representation of the product DTO.</returns>
        public Product ToEntity()
        {
            return new Product
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                SalePrice = SalePrice,
                PriceType = PriceType,
                ImageUrl = ImageUrl,
                Stock = Stock,
                ManufacturerId = ManufacturerId,
                CategoryId = CategoryId
            };
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is ProductDto product)
            {
                return Id == product.Id
                   && Name == product.Name
                   && Description == product.Description
                   && Price == product.Price
                   && SalePrice == product.SalePrice
                   && PriceType == product.PriceType
                   && ImageUrl == product.ImageUrl
                   && Stock == product.Stock
                   && ManufacturerId == product.ManufacturerId
                   && CategoryId == product.CategoryId;
            }

            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Provides extension methods for converting between <see cref="Product"/> and <see cref="ProductDto"/>.
    /// </summary>
    public static class ProductExtensions
    {
        /// <summary>
        /// Converts a product entity to its corresponding DTO.
        /// </summary>
        /// <param name="product">The product entity to convert.</param>
        /// <returns>The DTO representation of the product entity.</returns>
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SalePrice = product.SalePrice,
                PriceType = product.PriceType,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                ManufacturerId = product.ManufacturerId,
                ManufacturerName = product.Manufacturer?.Name,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name
            };
        }
    }
}
