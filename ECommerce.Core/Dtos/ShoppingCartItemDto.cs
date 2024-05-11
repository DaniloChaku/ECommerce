using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    /// <summary>
    /// Represents a shopping cart item data transfer object (DTO).
    /// </summary>
    public class ShoppingCartItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the shopping cart item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the customer associated with the shopping cart item.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the product associated with the shopping cart item.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product associated with the shopping cart item.
        /// </summary>
        public string? ProductName { get; set; }

        /// <summary>
        /// Gets or sets the price of the product associated with the shopping cart item.
        /// </summary>
        public decimal? ProductPrice { get; set; }

        /// <summary>
        /// Gets or sets the price type of the product associated with the shopping cart item.
        /// </summary>
        public PriceType? ProductPriceType { get; set; }

        /// <summary>
        /// Gets or sets the count of the product associated with the shopping cart item.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Count must not be less than 1")]
        public int Count { get; set; }

        /// <summary>
        /// Converts the shopping cart item DTO to its corresponding entity.
        /// </summary>
        /// <returns>The entity representation of the shopping cart item DTO.</returns>
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

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
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
    /// Provides extension methods for converting between <see cref="ShoppingCartItem"/> and <see cref="ShoppingCartItemDto"/>.
    /// </summary>
    public static class ShoppingCartItemExtensions
    {
        /// <summary>
        /// Converts a shopping cart item entity to its corresponding DTO.
        /// </summary>
        /// <param name="item">The shopping cart item entity to convert.</param>
        /// <returns>The DTO representation of the shopping cart item entity.</returns>
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
