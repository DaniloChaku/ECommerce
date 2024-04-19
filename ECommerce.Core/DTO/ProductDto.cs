using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTO
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Required]
        [Remote(controller: "Product", action: "IsProductNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Product with the same name already exists")]
        public string Name { get; set; }
        [MaxLength(400)]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Price")]
        [Range(typeof(decimal), "0.01", Constants.MaxDecimalValueString, 
            ParseLimitsInInvariantCulture = true, 
            ErrorMessage = "{0} should be greater than 0")]
        public decimal Price { get; set; }
        [Display(Name = "Sale Price")]
        [Range(typeof(decimal), "0.01", Constants.MaxDecimalValueString,
            ParseLimitsInInvariantCulture = true,
            ErrorMessage = "{0} should be greater than 0")]
        public decimal? SalePrice { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "The number of products in stock should not be negative")]
        public long Stock { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public Product ToEntity()
        {
            return new Product
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                SalePrice = SalePrice,
                ImageUrl = ImageUrl,
                Stock = Stock,
                ManufacturerId = ManufacturerId,
                CategoryId = CategoryId
            };
        }
    }

    public static class ProductExtensions
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SalePrice = product.SalePrice,
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
