using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.UI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ECommerce.Core.ServiceContracts.Category;
using ECommerce.Core.ServiceContracts.Manufacturer;
using Microsoft.AspNetCore.Hosting;
using ECommerce.Core.Enums;
using ECommerce.UI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce.Core.ServiceContracts.Image;
using ECommerce.Core.Settings;
using Microsoft.Extensions.Options;

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

        public ProductControllerTests()
        {
            _fixture = new Fixture();

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
                AllowedTypes = new List<ExtensionMapping>()
                {
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
                },
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

        private ProductController CreateProductController()
        {
            return new ProductController(_productGetterService,
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
            _productGetterServiceMock.Setup(t => t.GetAllAsync()).ReturnsAsync(new List<ProductDto>());

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

            _productGetterServiceMock.Setup(t => t.GetByIdAsync(
                It.IsAny<Guid>())).ReturnsAsync(expectedProduct);

            _categoryGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(categories);
            _categorySorterServiceMock.Setup(t => t.Sort(It.IsAny<IEnumerable<CategoryDto>>(), 
                It.IsAny<SortOrder>())).Returns(categories);

            _manufacturerGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(manufacturers);
            _manufacturerSorterServiceMock.Setup(t => t.Sort(
                It.IsAny<IEnumerable<ManufacturerDto>>(), It.IsAny<SortOrder>()))
                .Returns(manufacturers);

            var productController = CreateProductController();

            // Act
            var result = await productController.Upsert(null as Guid?);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertModel>(viewResult.Model);
            model.Categories.Should().NotBeEmpty();
            model.Manufacturers.Should().NotBeEmpty();
            model.Product.Should().BeEquivalentTo(expectedProduct);
        }

        [Fact]
        public async Task Upsert_ValidId_ReturnsViewResultWithProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedProduct = _fixture.Build<ProductDto>()
                .With(t => t.Id, productId)
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();
            _productGetterServiceMock.Setup(t => t.GetByIdAsync(
                productId)).ReturnsAsync(expectedProduct);

            var categories = _fixture.CreateMany<CategoryDto>(5).ToList();
            var manufacturers = _fixture.CreateMany<ManufacturerDto>(5).ToList();

            _categoryGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(categories);
            _categorySorterServiceMock.Setup(t => t.Sort(It.IsAny<IEnumerable<CategoryDto>>(),
                It.IsAny<SortOrder>())).Returns(categories);

            _manufacturerGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(manufacturers);
            _manufacturerSorterServiceMock.Setup(t => t.Sort(
                It.IsAny<IEnumerable<ManufacturerDto>>(), It.IsAny<SortOrder>()))
                .Returns(manufacturers);

            var productController = CreateProductController();

            // Act
            var result = await productController.Upsert(productId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertModel>(viewResult.Model);
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
            _productGetterServiceMock.Setup(t => t.GetByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            _categoryGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(categories);
            _categorySorterServiceMock.Setup(t => t.Sort(It.IsAny<IEnumerable<CategoryDto>>(),
                It.IsAny<SortOrder>())).Returns(categories);

            _manufacturerGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(manufacturers);
            _manufacturerSorterServiceMock.Setup(t => t.Sort(
                It.IsAny<IEnumerable<ManufacturerDto>>(), It.IsAny<SortOrder>()))
                .Returns(manufacturers);

            var productController = CreateProductController();

            // Act
            var result = await productController.Upsert(productId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductUpsertModel>(viewResult.Model);
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
            var expectedProduct = _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();
            var expectedVm = new ProductUpsertModel()
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
            var model = Assert.IsAssignableFrom<ProductUpsertModel>(viewResult.Model);
            model.Should().BeEquivalentTo(expectedVm);
        }

        [Fact]
        public async Task Upsert_EmptyProductDtoId_CallsAddAsync()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);
            var product = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();
            var vm = new ProductUpsertModel()
            {
                Product = product,
                Categories = categories,
                Manufacturers = manufacturers
            };

            var productController = CreateProductController();

            // Act
            _productAdderServiceMock.Setup(t => t.AddAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(product);

            var result = await productController.Upsert(vm);

            // Assert
            _productAdderServiceMock.Verify(t => t.AddAsync(product), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_NonEmptyProductDtoId_CallsUpdateAsync()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);
            var product = _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();
            var vm = new ProductUpsertModel()
            {
                Product = product,
                Categories = categories,
                Manufacturers = manufacturers
            };

            var productController = CreateProductController();

            // Act
            _productUpdaterServiceMock.Setup(t => t.UpdateAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(product);

            var result = await productController.Upsert(vm);

            // Assert
            _productUpdaterServiceMock.Verify(t => t.UpdateAsync(product), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_ValidIdAndNonEmptyImage_CallsUpdateAsyncAndUploadAsyncAndDelete()
        {
            // Arrange
            var categories = _fixture.CreateMany<SelectListItem>(5);
            var manufacturers = _fixture.CreateMany<SelectListItem>(5);

            var product = _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();

            var imageMock = new Mock<IFormFile>();
            imageMock.Setup(i => i.Length).Returns(1024);
            imageMock.Setup(i => i.FileName).Returns("image.jpg");
            imageMock.Setup(i => i.ContentType).Returns("image/jpeg");
            var image = imageMock.Object;

            var vm = new ProductUpsertModel()
            {
                Product = product,
                Categories = categories,
                Manufacturers = manufacturers,
                Image = image
            };

            var productController = CreateProductController();

            // Act
            _productUpdaterServiceMock.Setup(t => t.UpdateAsync(It.IsAny<ProductDto>()))
                .ReturnsAsync(product);
            _imageUploaderServiceMock.Setup(s => s.UploadAsync(It.IsAny<IFormFile>(),
                It.IsAny<string>())).ReturnsAsync("");
            _imageDeleterServiceMock.Setup(s => s.Delete(It.IsAny<string>()));

            var result = await productController.Upsert(vm);

            // Assert
            _productUpdaterServiceMock.Verify(t => t.UpdateAsync(product));
            _imageUploaderServiceMock.Verify(s => s.UploadAsync(image, 
                vm.Product.Id.ToString()), Times.Once);
            _imageDeleterServiceMock.Verify(s => s.Delete(It.IsAny<string>()), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ExceptionOccurred_ReturnsObjectResultWith500StatusCode()
        {
            // Arrange
            _productGetterServiceMock.Setup(t => t.GetAllAsync()).Throws(new Exception());

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
            List<ProductDto> productDtos = _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .CreateMany()
                .ToList();

            _productGetterServiceMock.Setup(t => t.GetAllAsync()).ReturnsAsync(productDtos);

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
        public async Task Delete_ExceptionOccurred_ReturnsOkResult()
        {
            // Arrange
            var id = Guid.NewGuid();

            _productDeleterServiceMock.Setup(t => t.DeleteAsync(id))
                .ReturnsAsync(false);

            var controller = CreateProductController();

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

            _productDeleterServiceMock.Setup(t => t.DeleteAsync(id))
                .ReturnsAsync(true);

            var controller = CreateProductController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion

        #region ValidateSameName

        [Fact]
        public async Task ValidateSameName_NewName_ReturnsTrue()
        {
            // Arrange
            _productGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(new List<ProductDto>());

            var product = _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();

            var productController = CreateProductController();

            // Act
            var result = await productController.ValidateSameName(product);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Fact]
        public async Task ValidateSameName_EmptyProductId_ReturnsTrue()
        {
            // Arrange
            var product = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();

            var productController = CreateProductController();

            // Act
            var result = await productController.ValidateSameName(product);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(true);
        }

        [Fact]
        public async Task ValidateSameName_ExistingName_ReturnsFalse()
        {
            // Arrange
            var product1 = _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();
            var product2 = _fixture.Build<ProductDto>()
                .With(t => t.Name, product1.Name)
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create();

            _productGetterServiceMock.Setup(t => t.GetAllAsync())
                .ReturnsAsync(new List<ProductDto>() { product1 });

            var productController = CreateProductController();

            // Act
            var result = await productController.ValidateSameName(product2);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            jsonResult.Value.Should().Be(false);
        }

        #endregion
    }
}
