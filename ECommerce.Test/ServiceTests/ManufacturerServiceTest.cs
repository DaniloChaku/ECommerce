﻿using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Manufacturer;
using ECommerce.Core.Services.Manufacturer;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Test.ServiceTests
{
    public class ManufacturerServiceTest
    {
        private readonly IManufacturerAdderService _manufacturerAdderService;
        private readonly IManufacturerDeleterService _manufacturerDeleterService;
        private readonly IManufacturerGetterService _manufacturerGetterService;
        private readonly IManufacturerSorterService _manufacturerSorterService;
        private readonly IManufacturerUpdaterService _manufacturerUpdaterService;

        private readonly Mock<IManufacturerRepository> _manufacturerRepositoryMock;
        private readonly IManufacturerRepository _manufacturerRepository;

        private readonly IFixture _fixture;

        public ManufacturerServiceTest()
        {
            _fixture = new Fixture();

            _manufacturerRepositoryMock = new Mock<IManufacturerRepository>();
            _manufacturerRepository = _manufacturerRepositoryMock.Object;

            _manufacturerAdderService = new ManufacturerAdderService(_manufacturerRepository);
            _manufacturerDeleterService = new ManufacturerDeleterService(_manufacturerRepository);
            _manufacturerGetterService = new ManufacturerGetterService(_manufacturerRepository);
            _manufacturerSorterService = new ManufacturerSorterService(_manufacturerRepository);
            _manufacturerUpdaterService = new ManufacturerUpdaterService(_manufacturerRepository);
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_NullName_ThrowsArgumentException()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, null as string)
                .Create();

            // Act
            var action = async () =>
            {
                await _manufacturerAdderService.AddAsync(manufacturerDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.NewGuid())
                .Create();

            // Act
            var action = async () =>
            {
                await _manufacturerAdderService.AddAsync(manufacturerDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_DuplicateName_ThrowsArgumentException()
        {
            // Arrange
            var manufacturerDto1 = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, "Test")
                .Create();
            var manufacturerDto2 = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, "Test")
                .Create();

            _manufacturerRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Manufacturer, bool>>?>())).ReturnsAsync(new List<Manufacturer>() { manufacturerDto1.ToEntity() });

            // Act
            var action = async () =>
            {
                await _manufacturerAdderService.AddAsync(manufacturerDto2);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsManufacturerDto()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.Empty).Create();
            _manufacturerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Manufacturer>()))
                .ReturnsAsync(true);

            // Act
            var result = await _manufacturerAdderService.AddAsync(manufacturerDto);

            manufacturerDto.Id = result.Id;

            // Assert
            result.Id.Should().NotBe(Guid.Empty);
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
            var manufacturers = _fixture.CreateMany<Manufacturer>(3);
            _manufacturerRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(manufacturers);

            // Act
            var result = await _manufacturerGetterService.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(manufacturers.Select(t => t.ToDto()));
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
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(manufacturerId))
                                   .ReturnsAsync(new Manufacturer { Id = manufacturerId });

            _manufacturerRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Manufacturer>()))
                                   .ReturnsAsync(true);

            // Ac
            var result = await _manufacturerDeleterService.DeleteAsync(manufacturerId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Manufacturer);

            // Act
            var result = await _manufacturerDeleterService.DeleteAsync(invalidId);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_EmptyId_ReturnsFalse()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();

            // Act
            var result = await _manufacturerUpdaterService.UpdateAsync(manufacturerDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_NullName_ReturnsFalse()
        {
            // Arrange
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, Guid.NewGuid())
                .With(t => t.Name, null as string)
                .Create();

            // Act
            var result = await _manufacturerUpdaterService.UpdateAsync(manufacturerDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var manufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, invalidId)
                .Create();

            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Manufacturer);

            // Act
            var result = await _manufacturerUpdaterService.UpdateAsync(manufacturerDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            var existingManufacturer = _fixture.Build<Manufacturer>()
                .With(t => t.Id, manufacturerId)
                .Create();
            var updatedManufacturerDto = _fixture.Build<ManufacturerDto>()
                .With(t => t.Id, manufacturerId)
                .Create();

            _manufacturerRepositoryMock.Setup(repo => repo.GetByIdAsync(manufacturerId))
                                   .ReturnsAsync(existingManufacturer);

            _manufacturerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Manufacturer>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _manufacturerUpdaterService.UpdateAsync(updatedManufacturerDto);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region SortAsync

        [Fact]
        public async Task SortAsync_SortAscending_ReturnsSortedCategories()
        {
            // Arrange
            var manufacturers = _fixture.CreateMany<ManufacturerDto>();

            var sortedManufacturers = manufacturers.OrderBy(t => t.Name);

            // Act
            var result = await _manufacturerSorterService.SortAsync(manufacturers);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedManufacturers);
        }

        [Fact]
        public async Task SortAsync_SortDescending_ReturnsSortedCategories()
        {
            // Arrange
            var manufacturers = _fixture.CreateMany<ManufacturerDto>();

            var sortedManufacturers = manufacturers.OrderByDescending(t => t.Name);

            // Act
            var result = await _manufacturerSorterService.SortAsync(manufacturers, SortOrder.DESC);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedManufacturers);
        }

        [Fact]
        public async Task SortAsync_EmptyCategories_ReturnsEmptyList()
        {
            // Arrange
            var manufacturers = new List<ManufacturerDto>();

            // Act
            var result = await _manufacturerSorterService.SortAsync(manufacturers);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion
    }
}