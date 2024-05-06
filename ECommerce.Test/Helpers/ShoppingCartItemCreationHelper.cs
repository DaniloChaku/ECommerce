using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.Helpers
{
    public class ShoppingCartItemCreationHelper
    {
        private readonly IFixture _fixture;

        public ShoppingCartItemCreationHelper(IFixture fixture)
        {
            _fixture = fixture;
        }

        public ShoppingCartItem CreateShoppingCartItem(bool isEmptyId = true)
        {
            if (isEmptyId)
            {
                return _fixture.Build<ShoppingCartItem>()
                    .With(i => i.Id, Guid.Empty)
                    .With(i => i.Product, ProductCreationHelper.CreateProduct(false))
                    .Create();
            }

            return _fixture.Build<ShoppingCartItem>()
                .With(i => i.Product, ProductCreationHelper.CreateProduct(false))
                .Create();
        }

        public List<ShoppingCartItem> CreateManyShoppingCartItems(int count = 10)
        {
            return _fixture.Build<ShoppingCartItem>()
                .With(i => i.Product, ProductCreationHelper.CreateProduct(false))
                .CreateMany(5).ToList();
        }
    }
}
