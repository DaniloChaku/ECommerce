using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Domain.Entities
{
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Count must not be less than 1")]
        public int Count { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is ShoppingCartItem other)
            {
                return other.Id == Id && other.Count == Count && other.CustomerId == CustomerId &&
                    other.ProductId == other.ProductId;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
