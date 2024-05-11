using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.ServiceContracts.Users;
using ECommerce.Tests.Helpers;
using ECommerce.UI.Controllers;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ECommerce.Tests.ControllerTests
{
    public class HomeControllerTests
    {
        private readonly IProductGetterService _productGetterService;
        private readonly IShoppingCartItemAdderService _shoppingCartItemAdderService;
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IShoppingCartItemUpdaterService _shoppingCartItemUpdaterService;
        private readonly IUserContextService _userContextService;

        private readonly Mock<IProductGetterService> _productGetterServiceMock;
        private readonly Mock<IShoppingCartItemAdderService> _shoppingCartItemAdderServiceMock;
        private readonly Mock<IShoppingCartItemGetterService> _shoppingCartItemGetterServiceMock;
        private readonly Mock<IShoppingCartItemUpdaterService> _shoppingCartItemUpdaterServiceMock;
        private readonly Mock<IUserContextService> _userContextServiceMock;

        private readonly IFixture _fixture;
        private readonly ProductCreationHelper _productCreationHelper;

        public HomeControllerTests()
        {
            _productGetterServiceMock = new Mock<IProductGetterService>();
            _shoppingCartItemAdderServiceMock = new Mock<IShoppingCartItemAdderService>();
            _shoppingCartItemGetterServiceMock = new Mock<IShoppingCartItemGetterService>();
            _shoppingCartItemUpdaterServiceMock = new Mock<IShoppingCartItemUpdaterService>();
            _userContextServiceMock = new Mock<IUserContextService>();

            _productGetterService = _productGetterServiceMock.Object;
            _shoppingCartItemAdderService = _shoppingCartItemAdderServiceMock.Object;
            _shoppingCartItemGetterService = _shoppingCartItemGetterServiceMock.Object;
            _shoppingCartItemUpdaterService = _shoppingCartItemUpdaterServiceMock.Object;
            _userContextService = _userContextServiceMock.Object;

            _fixture = new Fixture();
            _productCreationHelper = new ProductCreationHelper(_fixture);
        }

        public HomeController CreateController()
        {
            return new HomeController(_productGetterService, _shoppingCartItemAdderService,
                _shoppingCartItemGetterService, _shoppingCartItemUpdaterService,
                _userContextService);
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
            var products = _productCreationHelper.CreateManyProductDtos(productCount)
                .ToList();

            _productGetterServiceMock.Setup(s => s.GetBySearchQueryAsync(It.IsAny<string>()))
                .ReturnsAsync(products);

            var controller = CreateController();

            // Act
            var result = await controller.Index(page);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductPageViewModel>(viewResult.Model);
            model.CurrentPage.Should().Be(pageExpected);
            model.TotalPages.Should().Be(totalPagesExpected);
            model.PaginationStart.Should().Be(paginationStartExpected);
            model.PaginationEnd.Should().Be(paginationEndExpected);
        }
    }
}
