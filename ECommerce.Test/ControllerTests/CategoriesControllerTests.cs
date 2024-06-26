﻿using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Categories;
using ECommerce.UI.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace ECommerce.Tests.ControllerTests
{
    public class CategoriesControllerTests
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

        public CategoriesControllerTests()
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

        private CategoriesController CreateCategoryController()
        {
            return new CategoriesController(_categoryGetterService,
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
            _categoryGetterServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CategoryDto>());

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
            _categoryGetterServiceMock.Setup(s => s.GetByIdAsync(
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
                .With(m => m.Id, categoryId).Create();
            _categoryGetterServiceMock.Setup(s => s.GetByIdAsync(
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
            _categoryGetterServiceMock.Setup(s => s.GetByIdAsync(categoryId))
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
                .With(m => m.Id, Guid.Empty).Create();
            var categoryController = CreateCategoryController();

            // Act
            _categoryAdderServiceMock.Setup(s => s.AddAsync(It.IsAny<CategoryDto>()))
                .ReturnsAsync(categoryDto);

            var result = await categoryController.Upsert(categoryDto);

            // Assert
            _categoryAdderServiceMock.Verify(s => s.AddAsync(categoryDto), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_NonEmptyCategoryDtoId_CallsUpdateAsync()
        {
            // Arrange
            var categoryDto = _fixture.Create<CategoryDto>();

            var categoryController = CreateCategoryController();

            // Act
            _categoryUpdaterServiceMock.Setup(s => s.UpdateAsync(It.IsAny<CategoryDto>()))
                .ReturnsAsync(categoryDto);

            var result = await categoryController.Upsert(categoryDto);

            // Assert
            _categoryUpdaterServiceMock.Verify(s => s.UpdateAsync(categoryDto), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ExceptionOccurred_ReturnsObjectResultWith500StatusCode()
        {
            // Arrange
            _categoryGetterServiceMock.Setup(s => s.GetAllAsync()).Throws(new Exception());

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

            _categoryGetterServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categoryDtos);

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

            _categoryDeleterServiceMock.Setup(s => s.DeleteAsync(id))
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

            _categoryDeleterServiceMock.Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(true);

            var controller = CreateCategoryController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region IsCategoryNameUnique

        [Theory]
        [InlineData(new string[] { "A", "B", "C" }, "D")]
        public async Task IsCategoryNameUnique_EmptyIdAndNewName_ReturnsTrue(
            string[] existingCategoriesNames, string newName)
        {
            // Arrange
            var existingCategories = new List<CategoryDto>();
            foreach (var name in existingCategoriesNames)
            {
                var categoryDto = _fixture.Build<CategoryDto>()
                    .With(m => m.Name, name).Create();
                existingCategories.Add(categoryDto);
            }

            var category = _fixture.Build<CategoryDto>()
                .With(m => m.Id, Guid.Empty)
                .With(m => m.Name, newName).Create();
            var categoryController = CreateCategoryController();

            _categoryGetterServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(existingCategories);

            // Act
            var result = await categoryController.IsCategoryNameUnique(category);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Theory]
        [InlineData(new string[] { "A", "B", "C" }, "D")]
        public async Task IsCategoryNameUnique_ExistingIdAndNewName_ReturnsTrue(
            string[] existingCategoriesNames, string newName)
        {
            // Arrange
            var existingCategories = new List<CategoryDto>();
            foreach (var name in existingCategoriesNames)
            {
                var categoryDto = _fixture.Build<CategoryDto>()
                    .With(m => m.Name, name).Create();
                existingCategories.Add(categoryDto);
            }

            if (existingCategories.Count == 0)
            {
                existingCategories.Add(_fixture.Create<CategoryDto>());
            }

            var existingCategory = existingCategories[0];

            var category = _fixture.Build<CategoryDto>()
                .With(m => m.Id, existingCategory.Id)
                .With(m => m.Name, newName)
                .Create();

            _categoryGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCategory);
            _categoryGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(existingCategories);

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.IsCategoryNameUnique(category);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Fact]
        public async Task IsCategoryNameUnique_EmptyIdAndExistingName_ReturnsFalse()
        {
            // Arrange
            var existingCategory = _fixture.Create<CategoryDto>();
            var allCategories = new List<CategoryDto>() { existingCategory };
            var newCategory = _fixture.Build<CategoryDto>()
                .With(m => m.Id, Guid.Empty)
                .With(m => m.Name, existingCategory.Name)
                .Create();

            _categoryGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(allCategories);

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.IsCategoryNameUnique(newCategory);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(false);
        }

        [Fact]
        public async Task IsCategoryNameUnique_ExistingIdAndSameName_ReturnsTrue()
        {
            // Arrange
            var existingCategory = _fixture.Create<CategoryDto>();
            var newCategory = _fixture.Build<CategoryDto>()
                .With(m => m.Id, existingCategory.Id)
                .With(m => m.Name, existingCategory.Name)
                .Create();

            _categoryGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCategory);

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.IsCategoryNameUnique(newCategory);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Fact]
        public async Task IsCategoryNameUnique_ExistingIdAndExistingNameButDifferentFromPrevious_ReturnsFalse()
        {
            // Arrange
            var existingCategory1 = _fixture.Create<CategoryDto>();
            var existingCategory2 = _fixture.Create<CategoryDto>();
            var allCategories = new List<CategoryDto>() { existingCategory1, existingCategory2 };

            var newCategory = _fixture.Build<CategoryDto>()
                .With(m => m.Id, existingCategory1.Id)
                .With(m => m.Name, existingCategory2.Name)
                .Create();

            _categoryGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCategory1);
            _categoryGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(allCategories);

            var categoryController = CreateCategoryController();

            // Act
            var result = await categoryController.IsCategoryNameUnique(newCategory);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(false);
        }

        #endregion
    }
}
