using ECommerce.Core.Dtos;

namespace ECommerce.UI.Models
{
    /// <summary>
    /// Represents the view model used to display product details.
    /// </summary>
    public class ProductDetailViewModel
    {
        /// <summary>
        /// Gets or sets the product details.
        /// </summary>
        public required ProductDto Product { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// Gets or sets the ID of the shopping cart item related to the product, if any.
        /// </summary>
        public Guid? ShoppingCartItemId { get; set; }
    }
}
