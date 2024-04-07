using ECommerce.Core.DTO;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.UI.Views.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.UI.Controllers;

namespace ECommerce.Tests.ControllerTests
{
    public class ManufacturerControllerTests
    {
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerAdderService _manufacturerAdderService;
        private readonly IManufacturerUpdaterService _manufacturerUpdaterService;
        private readonly IManufacturerDeleterService _manufacturerDeleterService;
        private readonly ITempDataDictionary _tempData;

        private readonly Mock<IManufacturerGetterService> _manufacturerGetterServiceMock;
        private readonly Mock<IManufacturerAdderService> _manufacturerAdderServiceMock;
        private readonly Mock<IManufacturerUpdaterService> _manufacturerUpdaterServiceMock;
        private readonly Mock<IManufacturerDeleterService> _manufacturerDeleterServiceMock;
        private readonly Mock<ITempDataDictionary> _tempDataMock;

        private readonly IFixture _fixture;

        public ManufacturerControllerTests()
        {
            _fixture = new Fixture();

            _manufacturerGetterServiceMock = new Mock<IManufacturerGetterService>();
            _manufacturerAdderServiceMock = new Mock<IManufacturerAdderService>();
            _manufacturerUpdaterServiceMock = new Mock<IManufacturerUpdaterService>();
            _manufacturerDeleterServiceMock = new Mock<IManufacturerDeleterService>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _manufacturerGetterService = _manufacturerGetterServiceMock.Object;
            _manufacturerAdderService = _manufacturerAdderServiceMock.Object;
            _manufacturerUpdaterService = _manufacturerUpdaterServiceMock.Object;
            _manufacturerDeleterService = _manufacturerDeleterServiceMock.Object;
            _tempData = _tempDataMock.Object;
        }

        private ManufacturerController CreateManufacturerController()
        {
            return new ManufacturerController(_manufacturerGetterService,
                _manufacturerAdderService, _manufacturerUpdaterService, _manufacturerDeleterService)
            {
                TempData = _tempData
            };
        }

        #region Index

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            _manufacturerGetterServiceMock.Setup(t => t.GetAllAsync()).ReturnsAsync(new List<ManufacturerDto>());

            var manufacturerController = CreateManufacturerController();

            // Act
            var result = manufacturerController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        #endregion

        #region Upsert

        [Fact]
        public async Task Upsert_EmptyId_ReturnsViewResultWithEmptyManufacturer()
        {
            // Arrange
            _manufacturerGetterServiceMock.Setup(t => t.GetByIdAsync(
                It.IsAny<Guid>())).ReturnsAsync(default(ManufacturerDto));

            var manufacturerController = CreateManufacturerController();

            // Act
            var result = await manufacturerController.Upsert(null as Guid?);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ManufacturerDto>(viewResult.Model);
            model.Should().BeEquivalentTo(new ManufacturerDto());
        }

        [Fact]
        public async Task Upsert_ValidId_ReturnsViewResultWithManufacturer()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, manufacturerId).Create();
            _manufacturerGetterServiceMock.Setup(t => t.GetByIdAsync(
                manufacturerId)).ReturnsAsync(manufacturerDto);

            var manufacturerController = CreateManufacturerController();

            // Act
            var result = await manufacturerController.Upsert(manufacturerId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ManufacturerDto>(viewResult.Model);
            model.Should().BeEquivalentTo(manufacturerDto);
        }

        [Fact]
        public async Task Upsert_NonExistingId_ReturnsViewResultWithEmptyManufacturer()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            _manufacturerGetterServiceMock.Setup(t => t.GetByIdAsync(manufacturerId))
                .ReturnsAsync(default(ManufacturerDto));

            var manufacturerController = CreateManufacturerController();

            // Act
            var result = await manufacturerController.Upsert(manufacturerId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ManufacturerDto>(viewResult.Model);
            model.Should().BeEquivalentTo(new ManufacturerDto());
        }

        [Fact]
        public async Task Upsert_InvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var manufacturerController = CreateManufacturerController();

            manufacturerController.ModelState.AddModelError("Key", "Error message");
            var manufacturerDto = _fixture.Create<ManufacturerDto>();

            // Act
            var result = await manufacturerController.Upsert(manufacturerDto);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ManufacturerDto>(viewResult.Model);
            model.Should().BeEquivalentTo(manufacturerDto);
        }

        [Fact]
        public async Task Upsert_EmptyManufacturerDtoId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.Empty).Create();
            var manufacturerController = CreateManufacturerController();

            // Act
            _manufacturerAdderServiceMock.Setup(t => t.AddAsync(It.IsAny<ManufacturerDto>()))
                .ReturnsAsync(manufacturerDto);

            var result = await manufacturerController.Upsert(manufacturerDto);

            // Assert
            _manufacturerAdderServiceMock.Verify(t => t.AddAsync(manufacturerDto), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Upsert_NonEmptyManufacturerDtoId_CallsUpdateAsync()
        {
            // Arrange
            var manufacturerDto = _fixture.Create<ManufacturerDto>();

            var manufacturerController = CreateManufacturerController();

            // Act
            _manufacturerUpdaterServiceMock.Setup(t => t.UpdateAsync(It.IsAny<ManufacturerDto>()))
                .ReturnsAsync(manufacturerDto);

            var result = await manufacturerController.Upsert(manufacturerDto);

            // Assert
            _manufacturerUpdaterServiceMock.Verify(t => t.UpdateAsync(manufacturerDto), Times.Once);
            result.Should().BeOfType<RedirectToActionResult>();
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ExceptionOccurred_ReturnsObjectResultWith500StatusCode()
        {
            // Arrange
            _manufacturerGetterServiceMock.Setup(t => t.GetAllAsync()).Throws(new Exception());

            var controller = CreateManufacturerController();

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
            List<ManufacturerDto> manufacturerDtos = _fixture.CreateMany<ManufacturerDto>().ToList();

            _manufacturerGetterServiceMock.Setup(t => t.GetAllAsync()).ReturnsAsync(manufacturerDtos);

            var controller = CreateManufacturerController();

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

            _manufacturerDeleterServiceMock.Setup(t => t.DeleteAsync(id))
                .ReturnsAsync(false);

            var controller = CreateManufacturerController();

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

            _manufacturerDeleterServiceMock.Setup(t => t.DeleteAsync(id))
                .ReturnsAsync(true);

            var controller = CreateManufacturerController();

            // Act
            var result = await controller.Delete(id);

            // Assert
            result.Should().NotBe(null);
            result.Should().BeOfType<OkObjectResult>();
        }

        #endregion
    }
}
