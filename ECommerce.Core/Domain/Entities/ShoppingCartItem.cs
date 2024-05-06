using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.Helpers.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        [ValidateNever]
        public Product? Product { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Count must not be less than 1")]
        [LessThan("Product.Stock")]
        public int Count { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is ShoppingCartItem item)
            {
                return item.Id == Id && item.Count == Count && item.CustomerId == CustomerId &&
                    item.ProductId == item.ProductId;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
