using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.Helpers
{
    internal class ProductCreationHelper
    {
        private readonly IFixture _fixture;

        public ProductCreationHelper(IFixture fixture)
        {
            _fixture = fixture;
        }

        public IEnumerable<ProductDto> CreateManyProductDtos(int count = 10)
        {
            return _fixture.Build<ProductDto>()
                .With(p => p.Price, 10)
                .With(p => p.SalePrice, null as decimal?)
                .With(p => p.Stock, 10)
                .CreateMany(count);
        }

        public ProductDto CreateProductDto(bool isEmptyId = true, string? name = null)
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

        public IEnumerable<Product> CreateManyProducts(int count = 10)
        {
            return _fixture.Build<Product>()
                .With(p => p.Price, 10)
                .With(p => p.SalePrice, null as decimal?)
                .With(p => p.Stock, 10)
                .CreateMany(count);
        }

        public Product CreateProduct(bool isEmptyId = true, string? name = null)
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
