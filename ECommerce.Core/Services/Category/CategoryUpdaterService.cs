﻿using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Category
{
    public class CategoryUpdaterService : ICategoryUpdaterService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryUpdaterService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> UpdateAsync(CategoryDto categoryDto)
        {
            if (categoryDto is null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null");
            }

            if (string.IsNullOrEmpty(categoryDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(categoryDto.Name));
            }

            if (categoryDto.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(categoryDto.Id));
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
            if (existingCategory is null)
            {
                throw new ArgumentException("Category does not exist");
            }

            var category = categoryDto.ToEntity();

            if (!await _categoryRepository.UpdateAsync(category))
            {
                throw new InvalidOperationException("Failed to update category");
            }

            return true;
        }
    }
}
