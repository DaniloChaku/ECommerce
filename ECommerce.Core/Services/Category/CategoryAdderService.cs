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
    public class CategoryAdderService : ICategoryAdderService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAdderService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> AddAsync(CategoryDto categoryDto)
        {
            if (categoryDto is null)
            {
                throw new ArgumentNullException(nameof(categoryDto), "Category data cannot be null");
            }

            if (string.IsNullOrEmpty(categoryDto.Name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(categoryDto.Name));
            }

            if (categoryDto.Id != Guid.Empty)
            {
                throw new ArgumentException("Id must be empty", nameof(categoryDto.Id));
            }

            var existingCategories = await _categoryRepository.GetAllAsync(t => t.Name == categoryDto.Name);
            if (existingCategories.Any())
            {
                throw new ArgumentException("Category with the same name already exists");
            }

            categoryDto.Id = Guid.NewGuid();
            var category = categoryDto.ToEntity();

            if (!await _categoryRepository.AddAsync(category))
            {
                throw new InvalidOperationException("Failed to add category");
            }

            return true;
        }
    }
}
