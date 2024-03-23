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
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(400)]
        public string? Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Range(0, double.MaxValue)]
        public double? SalePrice { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public long Stock { get; set; }
        public Guid ManufacturerId { get; set; }
        [ForeignKey(nameof(ManufacturerId))]
        public Manufacturer? Manufacturer { get; set; }
        public Guid CategoryId { get; set; }
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
