using ECommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Dtos
{
    public class ManufacturerDto
    {
        public Guid Id { get; set; }
        [Remote(controller: "Manufacturers", action: "IsManufacturerNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Manufacturer with the same name already exists")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

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

    public static class ManufacturerExtensions
    {
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
