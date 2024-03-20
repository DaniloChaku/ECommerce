using ECommerce.Core.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Test.RepositoryTests
{
    public class ManufacturerRepositoryTest : IDisposable
    {
        private readonly IFixture _fixture;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public ManufacturerRepositoryTest()
        {
            _fixture = new Fixture();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ManufacturerRepositoryTests")
                .Options;
        }

        public void Dispose()
        {
            // Clean up the database after each test
            using (var context = new ApplicationDbContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        private ApplicationDbContext GetContext()
        {
            return new ApplicationDbContext(_dbContextOptions);
        }

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_EmptyDb_ReturnsAllManufacturers()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetAllAsync_NotEmptyDb_ReturnsAllManufacturers()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturers = _fixture.CreateMany<Manufacturer>().ToList();
                context.Manufacturers.AddRange(manufacturers);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEquivalentTo(manufacturers);
            }
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);

                // Act
                var result = await repository.GetByIdAsync(Guid.NewGuid());

                // Assert
                result.Should().Be(null);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsManufacturer()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();
                context.Manufacturers.Add(manufacturer);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(manufacturer.Id);

                // Assert
                result.Should().BeEquivalentTo(manufacturer);
            }
        }

        #endregion

        #region AddAsync

        [Fact]
        public async Task AddAsync_AddsEntityToDbSet_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();

                // Act
                var result = await repository.AddAsync(manufacturer);

                // Assert
                result.Should().BeTrue();
                context.Manufacturers.Should().Contain(manufacturer);
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturers = _fixture.CreateMany<Manufacturer>().ToList();

                // Act
                foreach (var Manufacturer in manufacturers)
                {
                    var result = await repository.AddAsync(Manufacturer);
                    result.Should().BeTrue();
                }

                // Assert
                context.Manufacturers.Should().BeEquivalentTo(manufacturers);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingManufacturer_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();
                context.Manufacturers.Add(manufacturer);
                await context.SaveChangesAsync();

                var updatedManufacturer = _fixture.Create<Manufacturer>();
                updatedManufacturer.Id = manufacturer.Id;

                // Act
                var result = await repository.UpdateAsync(updatedManufacturer);

                // Assert
                result.Should().BeTrue();
                context.Manufacturers.Should().Contain(updatedManufacturer);
            }
        }

        [Fact]
        public async Task UpdateAsync_NonExistentManufacturer_ReturnsFalse()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();

                // Act
                var result = await repository.UpdateAsync(manufacturer);

                // Assert
                result.Should().BeFalse();
                context.Manufacturers.Should().NotContain(manufacturer);
            }
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ExistingManufacturer_RemovesManufacturer()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();
                context.Manufacturers.Add(manufacturer);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.DeleteAsync(manufacturer);

                // Assert
                result.Should().BeTrue();
                context.Manufacturers.Should().NotContain(manufacturer);
            }
        }

        [Fact]
        public async Task DeleteAsync_NonExistentManufacturer_ReturnsFalse()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();

                // Act
                var result = await repository.DeleteAsync(manufacturer);

                // Assert
                result.Should().BeFalse();
            }
        }

        #endregion
    }
}
