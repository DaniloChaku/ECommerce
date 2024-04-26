﻿using ECommerce.Core.DTO;

namespace ECommerce.UI.Models
{
    public class ProductPageModel
    {
        public List<ProductDto> Products { get; set; } = [];
        public int CurrentPage { get; set; }
        public int PaginationStart { get; set; }  
        public int PaginationEnd { get; set; }
        public int TotalPages { get; set; }
    }
}