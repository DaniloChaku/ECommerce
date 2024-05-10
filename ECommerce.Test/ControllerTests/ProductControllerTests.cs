using ECommerce.Core.Dtos;
using ECommerce.Core.ServiceContracts.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ECommerce.Core.ServiceContracts.Categories;
using ECommerce.Core.ServiceContracts.Manufacturers;
using Microsoft.AspNetCore.Hosting;
using ECommerce.Core.Enums;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce.Core.ServiceContracts.Images;
using ECommerce.Core.Settings;
using Microsoft.Extensions.Options;
using ECommerce.Core.Domain.Entities;
using System.Drawing;
using ECommerce.UI.Areas.Admin.Controllers;
using ECommerce.Tests.Helpers;

namespace ECommerce.Tests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly IProductGetterService _productGetterService;
        private readonly IProductAdderService _productAdderService;
        private readonly IProductUpdaterService _productUpdaterService;
        private readonly IProductDeleterService _productDeleterService;

        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ICategorySorterService _categorySorterService;
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerSorterService _manufacturerSorterService;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IImageUploaderService _imageUploaderService;
        private readonly IImageDeleterService _imageDeleterService;
        private readonly IOptions<ImageUploadOptions> _imageUploadOptions;

        private readonly ITempDataDictionary _tempData;

        private readonly Mock<IProductGetterService> _productGetterServiceMock;
        private readonly Mock<IProductAdderService> _productAdderServiceMock;
        private readonly Mock<IProductUpdaterService> _productUpdaterServiceMock;
        private readonly Mock<IProductDeleterService> _productDeleterServiceMock;

        private readonly Mock<ICategoryGetterService> _categoryGetterServiceMock;
        private readonly Mock<ICategorySorterService> _categorySorterServiceMock;
        private readonly Mock<IManufacturerGetterService> _manufacturerGetterServiceMock;
        private readonly Mock<IManufacturerSorterService> _manufacturerSorterServiceMock;

        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        private readonly Mock<IImageUploaderService> _imageUploaderServiceMock;
        private readonly Mock<IImageDeleterService> _imageDeleterServiceMock;
        private readonly Mock<IOptions<ImageUploadOptions>> _imageUploadOptionsMock;

        private readonly Mock<ITempDataDictionary> _tempDataMock;

        private readonly IFixture _fixture;
        private readonly ProductCreationHelper _productCreationHelper;

        public ProductControllerTests()
        {
            _fixture = new Fixture();
            _productCreationHelper = new ProductCreationHelper(_fixture);

            _productGetterServiceMock = new Mock<IProductGetterService>();
            _productAdderServiceMock = new Mock<IProductAdderService>();
            _productUpdaterServiceMock = new Mock<IProductUpdaterService>();
            _productDeleterServiceMock = new Mock<IProductDeleterService>();

            _categoryGetterServiceMock = new Mock<ICategoryGetterService>();
            _categorySorterServiceMock = new Mock<ICategorySorterService>();
            _manufacturerGetterServiceMock = new Mock<IManufacturerGetterService>();
            _manufacturerSorterServiceMock = new Mock<IManufacturerSorterService>();

            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            _imageUploaderServiceMock = new Mock<IImageUploaderService>();
            _imageDeleterServiceMock = new Mock<IImageDeleterService>();
            var imageUploadOptions = new ImageUploadOptions()
            {
                AllowedTypes =
                [
                    new ExtensionMapping()
                    {
                        Extension = ".jpg",
                        ContentType = "image/jpeg"
                    },
                    new ExtensionMapping()
                    {
                        Extension = ".jpeg",
                        ContentType = "image/jpeg"
                    },
                    new ExtensionMapping()
                    {
                        Extension = ".png",
                        ContentType = "image/png"
                    },
                    new ExtensionMapping()
                    {
                        Extension = ".gif",
                        ContentType = "image/gif"
                    }
                ],
                MaxImageSize = 1024 * 1024 * 10,
            };

            _imageUploadOptionsMock = new Mock<IOptions<ImageUploadOptions>>();
            _imageUploadOptionsMock.Setup(m => m.Value).Returns(imageUploadOptions);

            _tempDataMock = new Mock<ITempDataDictionary>();

            _productGetterService = _productGetterServiceMock.Object;
            _productAdderService = _productAdderServiceMock.Object;
            _productUpdaterService = _productUpdaterServiceMock.Object;
            _productDeleterService = _productDeleterServiceMock.Object;

            _categoryGetterService = _categoryGetterServiceMock.Object;
            _categorySorterService = _categorySorterServiceMock.Object;
            _manufacturerGetterService = _manufacturerGetterServiceMock.Object;
            _manufacturerSorterService = _manufacturerSorterServiceMock.Object;

            _webHostEnvironment = _webHostEnvironmentMock.Object;

            _imageUploaderService = _imageUploaderServiceMock.Object;
            _imageDeleterService = _imageDeleterServiceMock.Object;
            _imageUploadOptions = _imageUploadOptionsMock.Object;

            _tempData = _tempDataMock.Object;
        }

        private ProductsController CreateProductController()
        {
            return new ProductsController(_productGetterService,
                _productAdderService, _productUpdaterService, _productDeleterService,
                _categoryGetterService, _categorySorterService, _manufacturerGetterService,
                _manufacturerSorterService, _webHostEnvironment, _imageUploaderService, 
                _imageDeleterService, _imageUploadOptions)
            {
                TempData = _tempData
            };
        }

        #region Index

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            _productGetterServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync([]);

            var productController = CreateProductController();

            // Act
            var result = productController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_NullId_ReturnsViewResultWithEmptyProduct()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryDto>(5).ToList();
            var manufacturers = _fixture.CreateMany<ManufacturerDto>(5).ToList();
            var expectedProduct = new ProductDto();

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(
                It.IsAny<Guid>())).ReturnsAsync(expectedProduct);

            _categoryGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(categories);
            _categorySorterServiceMock.Setup(s => s.Sort(It.IsAny<IEnumerable<CategoryDto>>(), 
                It.IsAny<SortOrder>())).Returns(categories);

            _manufacturerGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(manufacturers);
            _manufacturerSorterServiceMock.Setup(s => s.Sort(
                It.IsAny<IEnumerable<ManufacturerDto>>(), It.IsAny<SortOrder>()))
                .Returns(manufacturers);

            var productController = CreateProductController();

            // Act
            var result = await productController.Upsert(null as Guid?);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertViewModel>(viewResult.Model);
            model.Categories.Should().NotBeEmpty();
            model.Manufacturers.Should().NotBeEmpty();
            model.Product.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task Upsert_ValidId_ReturnsViewResultWithProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedProduct = _productCreationHelper.CreateProductDto();
            expectedProduct.Id = productId;

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(
                productId)).ReturnsAsync(expectedProduct);

            var categories = _fixture.CreateMany<CategoryDto>(5).ToList();
            var manufacturers = _fixture.CreateMany<ManufacturerDto>(5).ToList();

            _categoryGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(categories);
            _categorySorterServiceMock.Setup(s => s.Sort(It.IsAny<IEnumerable<CategoryDto>>(),
                It.IsAny<SortOrder>())).Returns(categories);

            _manufacturerGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(manufacturers);
            _manufacturerSorterServiceMock.Setup(s => s.Sort(
                It.IsAny<IEnumerable<ManufacturerDto>>(), It.IsAny<SortOrder>()))
                .Returns(manufacturers);

            var productController = CreateProductController();

            // Act
            var result = await productController.Upsert(productId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertViewModel>(viewResult.Model);
            model.Categories.Should().NotBeEmpty();
            model.Manufacturers.Should().NotBeEmpty();
            model.Product.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task Upsert_NonExistingId_ReturnsViewResultWithEmptyProduct()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryDto>(5).ToList();
            var manufacturers = _fixture.CreateMany<ManufacturerDto>(5).ToList();
            var expectedProduct = new ProductDto();

            var productId = Guid.NewGuid();
            _productGetterServiceMock.Setup(s => s.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            _categoryGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(categories);
            _categorySorterServiceMock.Setup(s => s.Sort(It.IsAny<IEnumerable<CategoryDto>>(),
                It.IsAny<SortOrder>())).Returns(categories);

            _manufacturerGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(manufacturers);
            _manufacturerSorterServiceMock.Setup(s => s.Sort(
                It.IsAny<IEnumerable<ManufacturerDto>>(), It.IsAny<SortOrder>()))
                .Returns(manufacturers);

            var productController = CreateProductController();

            // Act
            var result = await productController.Upsert(productId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertViewModel>(viewResult.Model);
            model.Categories.Should().NotBeEmpty();
            model.Manufacturers.Should().NotBeEmpty();
            model.Product.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task Upsert_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);
            var expectedProduct = _productCreationHelper.CreateProductDto(false);
            var expectedVm = new ProductUpsertViewModel()
            {
                Product = expectedProduct,
                Categories = categories,
                Manufacturers = manufacturers
            };

            var productController = CreateProductController();

            productController.ModelState.AddModelError("Key", "Error message");

            // Act
            var result = await productController.Upsert(expectedVm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertViewModel>(viewResult.Model);
            model.Should().BeEquivalentTo(expectedVm);
        }

        [Fact]
        public async Task Upsert_EmptyProductDtoId_CallsAddAsync()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);
            var product = _productCreationHelper.CreateProductDto();
            var vm = new ProductUpsertViewModel()
            {
                Product = product,
                Categories = categories,
                Manufacturers = manufacturers
            };

            var productController = CreateProductController();

            // Act
            _productAdderServiceMock.Setup(s => s.AddAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(product);

            var result = await productController.Upsert(vm);

            // Assert
            _productAdderServiceMock.Verify(s => s.AddAsync(product), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_NonEmptyProductDtoId_CallsUpdateAsync()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);
            var product = _productCreationHelper.CreateProductDto(false);
            var vm = new ProductUpsertViewModel()
            {
                Product = product,
                Categories = categories,
                Manufacturers = manufacturers
            };

            var productController = CreateProductController();

            // Act
            _productUpdaterServiceMock.Setup(s => s.UpdateAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(product);

            var result = await productController.Upsert(vm);

            // Assert
            _productUpdaterServiceMock.Verify(s => s.UpdateAsync(product), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_ValidIdAndNonEmptyImage_CallsUpdateAsyncAndUploadAsyncAndDelete()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);

            var product = _productCreationHelper.CreateProductDto(false);

            var imageMock = new Mock<IFormFile>();
            imageMock.Setup(i => i.Length).Returns(1024);
            imageMock.Setup(i => i.FileName).Returns("image.jpg");
            imageMock.Setup(i => i.ContentType).Returns("image/jpeg");
            var image = imageMock.Object;

            var vm = new ProductUpsertViewModel()
            {
                Product = product,
                Categories = categories,
                Manufacturers = manufacturers,
                Image = image
            };

            var productController = CreateProductController();

            // Act
            _productUpdaterServiceMock.Setup(s => s.UpdateAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(product);
            _imageUploaderServiceMock.Setup(s => s.UploadAsync(It.IsAny<IFormFile>(),
                It.IsAny<string>())).ReturnsAsync("");
            _imageDeleterServiceMock.Setup(s => s.DeleteImage(It.IsAny<string>()));

            var result = await productController.Upsert(vm);

            // Assert
            _productUpdaterServiceMock.Verify(s => s.UpdateAsync(product));
            _imageUploaderServiceMock.Verify(s => s.UploadAsync(image, 
                vm.Product.Id.ToString()), Times.Once);
            _imageDeleterServiceMock.Verify(s => s.DeleteImage(It.IsAny<string>()), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ExceptionOccurred_ReturnsObjectResultWith500StatusCode()
        {
            // Arrange
            _productGetterServiceMock.Setup(s => s.GetAllAsync()).Throws(new Exception());

            var controller = CreateProductController();

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
            var productDtos = _productCreationHelper.CreateManyProductDtos().ToList();

            _productGetterServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(productDtos);

            var controller = CreateProductController();

            // Act
            var result = await controller.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_ExceptionOccurred_ReturnsObjectResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productDto = new ProductDto();

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(productDto);
            _productDeleterServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var controller = CreateProductController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task Delete_DeletedSuccessfully_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var productDto = new ProductDto();

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(productDto);
            _productDeleterServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var controller = CreateProductController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_DeletedSuccessfullyWithImage_ReturnsOkResult()
        {
            // Arrange
            var productDto = _productCreationHelper.CreateProductDto(false);

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(productDto);
            _productDeleterServiceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            _imageDeleterServiceMock.Setup(s => s.DeleteImageFolder(It.IsAny<string>()));

            var controller = CreateProductController();

            // Act
            var result = await controller.Delete(productDto.Id);

            // Assert
            _imageDeleterServiceMock.Verify(s => s.DeleteImageFolder(It.IsAny<string>()), 
                Times.Once);
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region IsProductNameUnique

        [Theory]
        [InlineData(new string[] { "A", "B", "C" }, "D")]
        public async Task IsProductNameUnique_EmptyIdAndNewName_ReturnsTrue(
            string[] existingProductsNames, string newName)
        {
            // Arrange
            var existingProducts = new List<ProductDto>(); 
            foreach(var name in existingProductsNames)
            {
                var productDto = _productCreationHelper.CreateProductDto(false, name);
                existingProducts.Add(productDto);
            }

            var product = _productCreationHelper.CreateProductDto(false, newName); 
            var productController = CreateProductController();

            _productGetterServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(existingProducts);

            // Act
            var result = await productController.IsProductNameUnique(product);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true); 
        }

        [Theory]
        [InlineData(new string[] { "A", "B", "C" }, "D")]
        public async Task IsProductNameUnique_ExistingIdAndNewName_ReturnsTrue(
            string[] existingProductsNames, string newName)
        {
            // Arrange
            var existingProducts = new List<ProductDto>();
            foreach (var name in existingProductsNames)
            {
                var productDto = _productCreationHelper.CreateProductDto(false);
                existingProducts.Add(productDto);
            }

            if (existingProducts.Count == 0)
            {
                var productDto = _productCreationHelper.CreateProductDto(false);
                existingProducts.Add(productDto);
            }

            var existingProduct = existingProducts[0];

            var product = _productCreationHelper.CreateProductDto(false, newName);
            product.Id = existingProduct.Id;

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);
            _productGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(existingProducts);

            var productController = CreateProductController();

            // Act
            var result = await productController.IsProductNameUnique(product);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Fact]
        public async Task IsProductNameUnique_EmptyIdAndExistingName_ReturnsFalse()
        {
            // Arrange
            var existingProduct = _productCreationHelper.CreateProductDto(false);
            var allProducts = new List<ProductDto>() { existingProduct };
            var newProduct = _productCreationHelper.CreateProductDto(true, existingProduct.Name);

            _productGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(allProducts);

            var productController = CreateProductController();

            // Act
            var result = await productController.IsProductNameUnique(newProduct);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(false);
        }

        [Fact]
        public async Task IsProductNameUnique_ExistingIdAndSameName_ReturnsTrue()
        {
            // Arrange
            var existingProduct = _productCreationHelper.CreateProductDto(false);
            var newProduct = _productCreationHelper.CreateProductDto(true, existingProduct.Name);
            newProduct.Id = existingProduct.Id;

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct);

            var productController = CreateProductController();

            // Act
            var result = await productController.IsProductNameUnique(newProduct);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Fact]
        public async Task IsProductNameUnique_ExistingIdAndExistingNameButDifferentFromPrevious_ReturnsFalse()
        {
            // Arrange
            var existingProduct1 = _productCreationHelper.CreateProductDto(false);
            var existingProduct2 = _productCreationHelper.CreateProductDto(false);
            var allProducts = new List<ProductDto>() { existingProduct1, existingProduct2 };

            var newProduct = _productCreationHelper.CreateProductDto(true, existingProduct2.Name);
            newProduct.Id = existingProduct1.Id;

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingProduct1);
            _productGetterServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(allProducts);

            var productController = CreateProductController();

            // Act
            var result = await productController.IsProductNameUnique(newProduct);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(false);
        }

        #endregion

        #region HasReference

        [Theory]
        [InlineData("Category")]
        [InlineData("Manufacturer")]
        public async Task HasReference_ValidIdAndType_ReturnsOkObjectResult(string type)
        {
            // Arrange
            Guid validId = Guid.NewGuid();
            var products = new List<ProductDto>();

            _productGetterServiceMock.Setup(s => s.GetByCategoryAsync(validId))
                .ReturnsAsync(products);
            _productGetterServiceMock.Setup(s => s.GetByManufacturerAsync(validId))
                .ReturnsAsync(products);

            var controller = CreateProductController();

            // Act
            var result = await controller.HasReference(type, validId);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task HasReference_InvalidId_ReturnsBadRequestObjectResult()
        {
            // Arrange
            Guid emptyId = Guid.Empty;
            var type = "category";

            var controller = CreateProductController();

            // Act
            var result = await controller.HasReference(type, emptyId);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task HasReference_InvalidType_ReturnsBadRequestObjectResult()
        {
            // Arrange
            Guid validId = Guid.NewGuid();
            var type = "";

            var controller = CreateProductController();

            // Act
            var result = await controller.HasReference(type, validId);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion

        #region RemoveReference

        [Theory]
        [InlineData("category")]
        [InlineData("manufacturer")]
        public async Task RemoveReference_ValidTypeAndId_ReturnsOkResult(string type)
        {
            // Arrange
            Guid validId = Guid.NewGuid();
            var products = new List<ProductDto>();
            var productDto = new ProductDto();

            _productGetterServiceMock.Setup(s => s.GetByCategoryAsync(validId))
                .ReturnsAsync(products);
            _productGetterServiceMock.Setup(s => s.GetByManufacturerAsync(validId))
                .ReturnsAsync(products);
            _productUpdaterServiceMock.Setup(s => s.UpdateAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(productDto);

            var contoller = CreateProductController();

            // Act
            var result = await contoller.RemoveReference(type, validId);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task RemoveReference_InvalidId_ReturnsBadRequestObjectResult()
        {
            // Arrange
            Guid emptyId = Guid.Empty;
            var type = "category";

            var controller = CreateProductController();

            // Act
            var result = await controller.RemoveReference(type, emptyId);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RemoveReference_InvalidType_ReturnsBadRequestObjectResult()
        {
            // Arrange
            Guid validId = Guid.NewGuid();
            var type = "";

            var controller = CreateProductController();

            // Act
            var result = await controller.RemoveReference(type, validId);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        #endregion
    }
}
