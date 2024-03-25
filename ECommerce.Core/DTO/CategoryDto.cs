using ECommerce.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.DTO
{
    public class CategoryDto
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Category ToEntity()
        {
            return new Category 
            { 
                Id = Id,
                Name = Name 
            };
        }

        public override bool Equals(object? obj)
        {
            if (obj is CategoryDto categoryDto)
            {
                return Id == categoryDto.Id && Name == categoryDto.Name;
            } 

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class CategoryExtensions
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
