﻿using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Product
{
    public interface IProductSorterService
    {
        Task<IEnumerable<ProductDto>> SortAsync(IEnumerable<ProductDto> products,
            string? sortBy, SortOrder sortOrder = SortOrder.ASC, bool onSaleFirst = false);
    }
}
