using ECommerce.Core.Enums;
using ECommerce.Core.Helpers;
using ECommerce.Core.Helpers.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [RegularExpression(@"^\S.*\S$",
            ErrorMessage = "The {0} field must not start or end with a whitespace characters and must contain at least one letter.")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(400)]
        public string? Description { get; set; }
        [Range(typeof(decimal), "0.01", Constants.MaxDecimalValueString,
            ParseLimitsInInvariantCulture = true)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Range(typeof(decimal), "0.01", Constants.MaxDecimalValueString,
            ParseLimitsInInvariantCulture = true)]
        [LessThan(nameof(Price))]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }
        public PriceType PriceType { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [Range(0, long.MaxValue,
            ErrorMessage = "The number of products in stock must not be negative")]
        public long Stock { get; set; }
        public Guid? ManufacturerId { get; set; }
        [ForeignKey(nameof(ManufacturerId))]
        public Manufacturer? Manufacturer { get; set; }
        public Guid? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
