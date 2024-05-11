using ECommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Core.Dtos
{
    /// <summary>
    /// Represents a manufacturer data transfer object (DTO).
    /// </summary>
    public class ManufacturerDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the manufacturer.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the manufacturer.
        /// </summary>
        [Remote(controller: "Manufacturers", action: "IsManufacturerNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Manufacturer with the same name already exists")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the manufacturer.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Converts the manufacturer DTO to its corresponding entity.
        /// </summary>
        /// <returns>The entity representation of the manufacturer DTO.</returns>
        public Manufacturer ToEntity()
        {
            return new Manufacturer
            {
                Id = Id,
                Name = Name,
                Description = Description
            };
        }
    }

    /// <summary>
    /// Provides extension methods for converting between <see cref="Manufacturer"/> and <see cref="ManufacturerDto"/>.
    /// </summary>
    public static class ManufacturerExtensions
    {
        /// <summary>
        /// Converts a manufacturer entity to its corresponding DTO.
        /// </summary>
        /// <param name="manufacturer">The manufacturer entity to convert.</param>
        /// <returns>The DTO representation of the manufacturer entity.</returns>
        public static ManufacturerDto ToDto(this Manufacturer manufacturer)
        {
            return new ManufacturerDto
            {
                Id = manufacturer.Id,
                Name = manufacturer.Name,
                Description = manufacturer.Description
            };
        }
    }
}
