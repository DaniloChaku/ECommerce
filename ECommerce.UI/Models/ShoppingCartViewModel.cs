using ECommerce.Core.Dtos;

namespace ECommerce.UI.Models
{
    /// <summary>
    /// Represents the view model used for displaying the shopping cart.
    /// </summary>
    public class ShoppingCartViewModel
    {
        /// <summary>
        /// Gets or sets the list of items in the shopping cart.
        /// </summary>
        public List<ShoppingCartItemDto> Items { get; set; }

        /// <summary>
        /// Gets or sets the total price of all items in the shopping cart.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
