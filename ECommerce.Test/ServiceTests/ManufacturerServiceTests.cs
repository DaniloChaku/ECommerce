using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Manufacturers;
using ECommerce.Core.Services.Manufacturers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Test.ServiceTests
{
    public class ManufacturerServiceTests
    {
        private readonly IManufacturerAdderService _manufacturerAdderService;
        private readonly IManufacturerDeleterService _manufacturerDeleterService;
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerSorterService _manufacturerSorterService;
        private readonly IManufacturerUpdaterService _manufacturerUpdaterService;

        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly IManufacturerRepository _manufacturerRepository;

        private readonly IFixture _fixture;

        public ManufacturerServiceTests()
        {
            _fixture = new Fixture();

            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _manufacturerRepository = _manufacturerRepositoryMock.Object;

            _manufacturerAdderService = new ManufacturerAdderService(_manufacturerRepository);
            _manufacturerDeleterService = new ManufacturerDeleterService(_manufacturerRepository);
            _manufacturerGetterService = new ManufacturerGetterService(_manufacturerRepository);
            _manufacturerSorterService = new ManufacturerSorterService();
            _manufacturerUpdaterService = new ManufacturerUpdaterService(_manufacturerRepository);
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_NullManufacturerDto_ThrowsArgumentNullException()
        {
            // Arrange
            var manufacturerDto = null as ManufacturerDto;

            // Act
            var action = async () =>
            {
                await _manufacturerAdderService.AddAsync(manufacturerDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, Guid.NewGuid())
                .Create();

            // Act
            var action = async () =>
            {
                await _manufacturerAdderService.AddAsync(manufacturerDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_DuplicateName_ThrowsArgumentException()
        {
            // Arrange
            var manufacturerDto1 = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, Guid.Empty)
                .With(m => m.Name, "Test")
                .Create();
            var manufacturerDto2 = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, Guid.Empty)
                .With(m => m.Name, "Test")
                .Create();

            _manufacturerRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Manufacturer, bool>>?>())).ReturnsAsync(new List<Manufacturer>() { manufacturerDto1.ToEntity() });

            // Act
            var action = async () =>
            {
                await _manufacturerAdderService.AddAsync(manufacturerDto2!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsAddedManufacturerDto()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, Guid.Empty).Create();
            var addedManufacturer = manufacturerDto.ToEntity();

            _manufacturerRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Manufacturer, bool>>?>()))
                                   .ReturnsAsync(new List<Manufacturer>());
            _manufacturerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Manufacturer>()))
                .ReturnsAsync(addedManufacturer);

            // Act
            var result = await _manufacturerAdderService.AddAsync(manufacturerDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(manufacturerDto);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            _manufacturerRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(new List<Manufacturer>());

            // Act
            var result = await _manufacturerGetterService.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_NonEmptyDb_ReturnsManufacturerList()
        {
            // Arrange
            var manufacturers = _fixture.CreateMany<Manufacturer>(3).ToList();
            _manufacturerRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(manufacturers);

            // Act
            var result = await _manufacturerGetterService.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(manufacturers.Select(m => m.ToDto()));
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Manufacturer);

            // Act
            var result = await _manufacturerGetterService.GetByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsManufacturerDto()
        {
            // Arrange
            var manufacturer = _fixture.Create<Manufacturer>();
            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(manufacturer.Id))
                                   .ReturnsAsync(manufacturer);

            // Act
            var result = await _manufacturerGetterService.GetByIdAsync(manufacturer.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(manufacturer.ToDto());
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Manufacturer);

            // Act
            var action = async () =>
            {
                await _manufacturerDeleterService.DeleteAsync(invalidId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            var existingManufacturer = _fixture.Build<Manufacturer>()
                .With(m => m.Id, manufacturerId).Create();

            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(manufacturerId))
                                   .ReturnsAsync(existingManufacturer);

            _manufacturerRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Manufacturer>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _manufacturerDeleterService.DeleteAsync(manufacturerId);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_NullManufacturerDto_ThrowsArgumentNullException()
        {
            // Arrange
            var manufacturerDto = null as ManufacturerDto;

            // Act
            var action = async () =>
            {
                await _manufacturerUpdaterService.UpdateAsync(manufacturerDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_EmptyId_ThrowsArgumentException()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, Guid.Empty)
                .Create();

            // Act
            var action = async () =>
            {
                await _manufacturerUpdaterService.UpdateAsync(manufacturerDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, invalidId)
                .Create();

            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Manufacturer);

            // Act
            var action = async () =>
            {
                await _manufacturerUpdaterService.UpdateAsync(manufacturerDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ReturnsUpdatedManufacturerDto()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            var existingManufacturer = _fixture.Build<Manufacturer>()
                .With(m => m.Id, manufacturerId)
                .Create();
            var updatedManufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(m => m.Id, manufacturerId)
                .Create();
            var updatedManufacturer = updatedManufacturerDto.ToEntity();

            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(manufacturerId))
                                   .ReturnsAsync(existingManufacturer);

            _manufacturerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Manufacturer>()))
                                   .ReturnsAsync(updatedManufacturer);

            // Act
            var result = await _manufacturerUpdaterService.UpdateAsync(updatedManufacturerDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedManufacturerDto);
        }

        #endregion

        #region Sort

        [Fact]
        public void Sort_SortAscending_ReturnsSortedCategories()
        {
            // Arrange
            var manufacturers = _fixture.CreateMany<ManufacturerDto>();

            var sortedManufacturers = manufacturers.OrderBy(m => m.Name);

            // Act
            var result = _manufacturerSorterService.Sort(manufacturers);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedManufacturers);
        }

        [Fact]
        public void Sort_SortDescending_ReturnsSortedCategories()
        {
            // Arrange
            var manufacturers = _fixture.CreateMany<ManufacturerDto>();

            var sortedManufacturers = manufacturers.OrderByDescending(m => m.Name);

            // Act
            var result = _manufacturerSorterService.Sort(manufacturers, SortOrder.DESC);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedManufacturers);
        }

        [Fact]
        public void Sort_EmptyCategories_ReturnsEmptyList()
        {
            // Arrange
            var manufacturers = new List<ManufacturerDto>();

            // Act
            var result = _manufacturerSorterService.Sort(manufacturers);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion
    }
}
