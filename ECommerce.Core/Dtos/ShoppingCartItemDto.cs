using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Enums;
using ECommerce.Core.Helpers.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Dtos
{
    public class ShoppingCartItemDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public PriceType? ProductPriceType { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Count must not be less than 1")]
        public int Count { get; set; }

        public ShoppingCartItem ToEntity()
        {
            return new ShoppingCartItem
            {
                Id = Id,
                CustomerId = CustomerId,
                ProductId = ProductId,
                Count = Count
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is ShoppingCartItemDto other)
            {
                return other.Id == Id && 
                    other.CustomerId == CustomerId &&
                    other.ProductId == ProductId && 
                    other.ProductName == ProductName &&
                    other.ProductPrice == ProductPrice &&
                    other.ProductPriceType == ProductPriceType &&
                    other.Count == Count;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class ShoppingCartItemExtensions
    {
        public static ShoppingCartItemDto ToDto(this ShoppingCartItem item)
        {
            return new ShoppingCartItemDto()
            {
                Id = item.Id,
                CustomerId = item.CustomerId,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                ProductPrice = item.Product?.SalePrice ?? item.Product?.Price,
                ProductPriceType = item.Product?.PriceType,
                Count = item.Count
            };
        }
    }
}
