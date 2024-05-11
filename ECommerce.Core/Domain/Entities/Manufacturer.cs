using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Manufacturer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Manufacturer manufacturer)
            {
                return Id == manufacturer.Id && Name == manufacturer.Name
                    && Description == manufacturer.Description;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
