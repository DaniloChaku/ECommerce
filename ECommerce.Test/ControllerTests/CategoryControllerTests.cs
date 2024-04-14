using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Category;
using ECommerce.UI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.ControllerTests
{
    public class CategoryControllerTests
    {
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ICategoryAdderService _categoryAdderService;
        private readonly ICategoryUpdaterService _categoryUpdaterService;
        private readonly ICategoryDeleterService _categoryDeleterService;
        private readonly ITempDataDictionary _tempData;

        private readonly Mock<ICategoryGetterService> _categoryGetterServiceMock;
        private readonly Mock<ICategoryAdderService> _categoryAdderServiceMock;
        private readonly Mock<ICategoryUpdaterService> _categoryUpdaterServiceMock;
        private readonly Mock<ICategoryDeleterService> _categoryDeleterServiceMock;
        private readonly Mock<ITempDataDictionary> _tempDataMock;

        private readonly IFixture _fixture;

        public CategoryControllerTests()
        {
            _fixture = new Fixture();

            _categoryGetterServiceMock = new Mock<ICategoryGetterService>();
            _categoryAdderServiceMock = new Mock<ICategoryAdderService>();
            _categoryUpdaterServiceMock = new Mock<ICategoryUpdaterService>();
            _categoryDeleterServiceMock = new Mock<ICategoryDeleterService>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _categoryGetterService = _categoryGetterServiceMock.Object;
            _categoryAdderService = _categoryAdderServiceMock.Object;
            _categoryUpdaterService = _categoryUpdaterServiceMock.Object;
            _categoryDeleterService = _categoryDeleterServiceMock.Object;
            _tempData = _tempDataMock.Object;
        }

        private CategoryController CreateCategoryController()
        {
            return new CategoryController(_categoryGetterService,
                _categoryAdderService, _categoryUpdaterService, _categoryDeleterService)
            {
                TempData = _tempData
            };
        }

        #region Index

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            _categoryGetterServiceMock.Setup(t => t.GetAllAsync()).ReturnsAsync(new List<CategoryDto>());

            var categoryController = CreateCategoryController();

            // Act
            var result = categoryController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_EmptyId_ReturnsViewResultWithEmptyCategory()
        {
            // Arrange
            _categoryGetterServiceMock.Setup(t => t.GetByIdAsync(
                It.IsAny<Guid>())).ReturnsAsync(default(CategoryDto));

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.Upsert(null as Guid?);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDto>(viewResult.Model);
            model.Should().BeEquivalentTo(new CategoryDto());
        }

        [Fact]
        public async Task Upsert_ValidId_ReturnsViewResultWithCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var categoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, categoryId).Create();
            _categoryGetterServiceMock.Setup(t => t.GetByIdAsync(
                categoryId)).ReturnsAsync(categoryDto);

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.Upsert(categoryId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDto>(viewResult.Model);
            model.Should().BeEquivalentTo(categoryDto);
        }

        [Fact]
        public async Task Upsert_NonExistingId_ReturnsViewResultWithEmptyCategory()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            _categoryGetterServiceMock.Setup(t => t.GetByIdAsync(categoryId))
                .ReturnsAsync(default(CategoryDto));

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.Upsert(categoryId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDto>(viewResult.Model);
            model.Should().BeEquivalentTo(new CategoryDto());
        }

        [Fact]
        public async Task Upsert_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var categoryController = CreateCategoryController();

            categoryController.ModelState.AddModelError("Key", "Error message");
            var categoryDto = _fixture.Create<CategoryDto>();

            // Act
            var result = await categoryController.Upsert(categoryDto);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDto>(viewResult.Model);
            model.Should().BeEquivalentTo(categoryDto);
        }

        [Fact]
        public async Task Upsert_EmptyCategoryDtoId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var categoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, Guid.Empty).Create();
            var categoryController = CreateCategoryController();

            // Act
            _categoryAdderServiceMock.Setup(t => t.AddAsync(It.IsAny<CategoryDto>()))
                .ReturnsAsync(categoryDto);

            var result = await categoryController.Upsert(categoryDto);

            // Assert
            _categoryAdderServiceMock.Verify(t => t.AddAsync(categoryDto), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_NonEmptyCategoryDtoId_CallsUpdateAsync()
        {
            // Arrange
            var categoryDto = _fixture.Create<CategoryDto>();

            var categoryController = CreateCategoryController();

            // Act
            _categoryUpdaterServiceMock.Setup(t => t.UpdateAsync(It.IsAny<CategoryDto>()))
                .ReturnsAsync(categoryDto);

            var result = await categoryController.Upsert(categoryDto);

            // Assert
            _categoryUpdaterServiceMock.Verify(t => t.UpdateAsync(categoryDto), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ExceptionOccurred_ReturnsObjectResultWith500StatusCode()
        {
            // Arrange
            _categoryGetterServiceMock.Setup(t => t.GetAllAsync()).Throws(new Exception());

            var controller = CreateCategoryController();

            // Act
            var result = await controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ObjectResult>().Which
                .StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }


        [Fact]
        public async Task GetAll_RetrievedSuccessfully_ReturnsOkObjectResultWithData()
        {
            // Arrange
            List<CategoryDto> categoryDtos = _fixture.CreateMany<CategoryDto>().ToList();

            _categoryGetterServiceMock.Setup(t => t.GetAllAsync()).ReturnsAsync(categoryDtos);

            var controller = CreateCategoryController();

            // Act
            var result = await controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_ExceptionOccurred_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            _categoryDeleterServiceMock.Setup(t => t.DeleteAsync(id))
                .ReturnsAsync(false);

            var controller = CreateCategoryController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_DeletedSuccessfully_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            _categoryDeleterServiceMock.Setup(t => t.DeleteAsync(id))
                .ReturnsAsync(true);

            var controller = CreateCategoryController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion
    }
}
