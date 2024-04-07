using ECommerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTO
{
    public class ManufacturerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
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
