using ECommerce.Core.Enums;
using ECommerce.Core.Helpers;
using ECommerce.Core.Helpers.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Domain.Entities
{
    /// <summary>
    /// Represents a product entity in the database.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the primary key for the Product entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [RegularExpression(@"^\S(.*\S)?$",
            ErrorMessage = "The {0} field must not start or end with a whitespace characters and must contain at least one letter.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the product.
        /// </summary>
        [MaxLength(400)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product.
        /// </summary>
        [Range(typeof(decimal), "0.01", Constants.MAX_DECIMAL_VALUE_STRING,
            ParseLimitsInInvariantCulture = true)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the sale price of the product.
        /// </summary>
        [Range(typeof(decimal), "0.01", Constants.MAX_DECIMAL_VALUE_STRING,
            ParseLimitsInInvariantCulture = true)]
        [LessThan(nameof(Price))]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        /// <summary>
        /// Gets or sets the type of pricing for the product.
        /// </summary>
        public PriceType PriceType { get; set; }

        /// <summary>
        /// Gets or sets the URL of the product image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the number of products in stock.
        /// </summary>
        [Required]
        [Range(0, long.MaxValue,
            ErrorMessage = "The number of products in stock must not be negative")]
        public long Stock { get; set; }

        /// <summary>
        /// Gets or sets the ID of the manufacturer associated with the product.
        /// </summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer associated with the product.
        /// </summary>
        [ForeignKey(nameof(ManufacturerId))]
        public Manufacturer? Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the ID of the category associated with the product.
        /// </summary>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category associated with the product.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Product product)
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
}
