﻿using ECommerce.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.Helpers
{
    internal static class ProductCreationHelper
    {
        private static readonly IFixture _fixture = new Fixture();

        public static IEnumerable<ProductDto> CreateManyProductDtos(int count)
        {
            return _fixture.Build<ProductDto>()
                .With(p => p.Price, 10)
                .With(p => p.SalePrice, null as decimal?)
                .With(p => p.Stock, 10)
                .CreateMany(count);
        }
    }
}