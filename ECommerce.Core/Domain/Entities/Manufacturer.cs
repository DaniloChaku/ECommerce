using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Domain.Entities
{
    /// <summary>
    /// Represents a manufacturer entity in the database.
    /// </summary>
    [Index(nameof(Name), IsUnique = true)]
    public class Manufacturer
    {
        /// <summary>
        /// Gets or sets the primary key for the Manufacturer entity.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the manufacturer.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Manufacturer manufacturer)
            {
                return Id == manufacturer.Id && Name == manufacturer.Name
                    && Description == manufacturer.Description;
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
}
