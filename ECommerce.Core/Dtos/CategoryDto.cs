using ECommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Core.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        [Remote(controller: "Categories", action: "IsCategoryNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Category with the same name already exists")]
        public string Name { get; set; } = string.Empty;

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
