using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Tests.Helpers;
using ECommerce.UI.Controllers;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly IProductGetterService _productGetterService;
        private readonly Mock<IProductGetterService> _productGetterServiceMock;

        private readonly IFixture _fixture;

        public HomeControllerTests()
        {
            _productGetterServiceMock = new Mock<IProductGetterService>();

            _productGetterService = _productGetterServiceMock.Object;

            _fixture = new Fixture();
        }

        public HomeController CreateController()
        {
            return new HomeController(_productGetterService);
        }

        [Theory]
        [InlineData(0, 4, 1, 1, 1, 1)]
        [InlineData(20, 2, 2, 2, 1, 2)]
        [InlineData(20, 5, 1, 2, 1, 2)]
        [InlineData(101, 4, 4, 11, 2, 6)]
        [InlineData(101, 2, 2, 11, 1, 4)]
        [InlineData(101, 11, 11, 11, 9, 11)]
        public async Task Index_ReturnsViewModelWithProductPageModel(int productCount, int page, 
            int pageExpected, int totalPagesExpected, int paginationStartExpected, int paginationEndExpected)
        {
            // Arrange
            var products = ProductCreationHelper.CreateManyProductDtos(productCount)
                .ToList();

            _productGetterServiceMock.Setup(s => s.GetBySearchQueryAsync(It.IsAny<string>()))
                .ReturnsAsync(products);

            var controller = CreateController();

            // Act
            var result = await controller.Index(page);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductPageModel>(viewResult.Model);
            model.CurrentPage.Should().Be(pageExpected);
            model.TotalPages.Should().Be(totalPagesExpected);
            model.PaginationStart.Should().Be(paginationStartExpected);
            model.PaginationEnd.Should().Be(paginationEndExpected);
        }
    }
}
