using ECommerce.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Core.Dtos
{
    /// <summary>
    /// Represents a category data transfer object (DTO).
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [Remote(controller: "Categories", action: "IsCategoryNameUnique",
            AdditionalFields = nameof(Id),
            ErrorMessage = "Category with the same name already exists")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Converts the DTO to its corresponding entity.
        /// </summary>
        /// <returns>The entity representation of the DTO.</returns>
        public Category ToEntity()
        {
            return new Category
            {
                Id = Id,
                Name = Name
            };
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is CategoryDto categoryDto)
            {
                return Id == categoryDto.Id && Name == categoryDto.Name;
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

    /// <summary>
    /// Provides extension methods for converting between <see cref="Category"/> and <see cref="CategoryDto"/>.
    /// </summary>
    public static class CategoryExtensions
    {
        /// <summary>
        /// Converts a category entity to its corresponding DTO.
        /// </summary>
        /// <param name="category">The category entity to convert.</param>
        /// <returns>The DTO representation of the category entity.</returns>
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
