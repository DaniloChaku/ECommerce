using ECommerce.Core.Dtos;

namespace ECommerce.UI.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
