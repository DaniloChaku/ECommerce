using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Dtos;
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

        public static ProductDto CreateProductDto(bool isEmptyId = true, string? name = null)
        {
            var product = _fixture.Build<ProductDto>()
                .With(p => p.Price, 10)
                .With(p => p.SalePrice, null as decimal?)
                .With(p => p.Stock, 10)
                .Create();

            if (isEmptyId)
            {
                product.Id = Guid.Empty;
            }

            if (name is not null)
            {
                product.Name = name;
            }

            return product;
        }

        public static IEnumerable<Product> CreateManyProducts(int count)
        {
            return _fixture.Build<Product>()
                .With(p => p.Price, 10)
                .With(p => p.SalePrice, null as decimal?)
                .With(p => p.Stock, 10)
                .CreateMany(count);
        }

        public static Product CreateProduct(bool isEmptyId = true, string? name = null)
        {
            var product = _fixture.Build<Product>()
                .With(p => p.Price, 10)
                .With(p => p.SalePrice, null as decimal?)
                .With(p => p.Stock, 10)
                .Create();

            if (isEmptyId)
            {
                product.Id = Guid.Empty;   
            }
            
            if (name is not null)
            {
                product.Name = name;
            }

            return product;
        }
    }
}
