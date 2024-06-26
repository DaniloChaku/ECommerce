﻿using ECommerce.Core.Domain.Entities;

namespace ECommerce.Tests.Helpers
{
    public class ShoppingCartItemCreationHelper
    {
        private readonly IFixture _fixture;
        private readonly ProductCreationHelper _productCreationHelper;

        public ShoppingCartItemCreationHelper(IFixture fixture)
        {
            _fixture = fixture;
            _productCreationHelper = new ProductCreationHelper(fixture);
        }

        public ShoppingCartItem CreateShoppingCartItem(bool isEmptyId = true)
        {
            if (isEmptyId)
            {
                return _fixture.Build<ShoppingCartItem>()
                    .With(i => i.Id, Guid.Empty)
                    .With(i => i.Product, _productCreationHelper.CreateProduct(false))
                    .Create();
            }

            return _fixture.Build<ShoppingCartItem>()
                .With(i => i.Product, _productCreationHelper.CreateProduct(false))
                .Create();
        }

        public List<ShoppingCartItem> CreateManyShoppingCartItems(int count = 10)
        {
            return _fixture.Build<ShoppingCartItem>()
                .With(i => i.Product, _productCreationHelper.CreateProduct(false))
                .CreateMany(5).ToList();
        }
    }
}
