using ECommerce.Core.Dtos;

namespace ECommerce.UI.Models
{
    public class ProductDetailViewModel
    {
        public required ProductDto Product { get; set; }
        public int Count { get; set; } = 1;
        public Guid? ShoppingCartItemId { get; set; }
    }
}
