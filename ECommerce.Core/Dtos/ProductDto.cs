﻿using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Enums;
using ECommerce.Core.Helpers;
using ECommerce.Core.Helpers.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [RegularExpression(@"^\S(.*\S)?$", 
            ErrorMessage = "The {0} field must not start or end with a whitespace characters and must contain at least one letter.")]
        [Remote(controller: "Product", action: "IsProductNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Product with the same name already exists")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(400)]
        public string? Description { get; set; }
        [Display(Name = "Price")]
        [Range(typeof(decimal), "0.01", Constants.MAX_DECIMAL_VALUE_STRING, 
            ParseLimitsInInvariantCulture = true, 
            ErrorMessage = "{0} must be greater than 0")]
        public decimal Price { get; set; }
        [Display(Name = "Sale Price")]
        [Range(typeof(decimal), "0.01", Constants.MAX_DECIMAL_VALUE_STRING,
            ParseLimitsInInvariantCulture = true,
            ErrorMessage = "{0} must be greater than 0")]
        [LessThan(nameof(Price), 
            ErrorMessage = "{0} must be less than {1}")]
        public decimal? SalePrice { get; set; }
        public PriceType PriceType { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        [Range(0, long.MaxValue, 
            ErrorMessage = "The number of products in stock must not be negative")]
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
                PriceType = PriceType,
                ImageUrl = ImageUrl,
                Stock = Stock,
                ManufacturerId = ManufacturerId,
                CategoryId = CategoryId
            };
        }

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
