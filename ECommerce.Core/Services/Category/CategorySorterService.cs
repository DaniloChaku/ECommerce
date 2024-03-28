﻿using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Category
{
    public class CategorySorterService : ICategorySorterService
    {
        public List<CategoryDto> Sort(IEnumerable<CategoryDto> categories, 
            SortOrder sortOrder = SortOrder.ASC)
        {
            if (sortOrder == SortOrder.ASC)
            {
                return categories.OrderBy(t => t.Name).ToList();
            }

            return categories.OrderByDescending(t => t.Name).ToList();
        }
    }
}
