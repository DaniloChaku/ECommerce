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

        [Fact]
        public async Task GetAllAsync_WithFilter_ReturnsMatchingManufacturers()
        {

            using (var context = GetContext())
            {
                // Arrange
                var fixture = new Fixture();
                var repository = new ManufacturerRepository(context);

                var manufacturer1 = fixture.Build<Manufacturer>()
                    .With(c => c.Name, "ABC").Create();
                context.Manufacturers.Add(manufacturer1);

                var manufacturer2 = fixture.Build<Manufacturer>()
                    .With(c => c.Name, "P Corp").Create();
                context.Manufacturers.Add(manufacturer2);

                var manufacturer3 = fixture.Build<Manufacturer>()
                    .With(c => c.Name, "BCA").Create();
                context.Manufacturers.Add(manufacturer3);

                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync(c => c.Name == "BCA");

                // Assert
                result.Should().ContainSingle(c => c.Id == manufacturer3.Id);
                result.Should().NotContain(c => c.Id == manufacturer2.Id);
                result.Should().NotContain(c => c.Id == manufacturer1.Id);
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
        public async Task AddAsync_AddsEntityToDbSet_ReturnsAddedManufacturer()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturer = _fixture.Create<Manufacturer>();

                // Act
                var result = await repository.AddAsync(manufacturer);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().NotBe(Guid.Empty);
                context.Manufacturers.Should().Contain(result);
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsAddedManufacturers()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ManufacturerRepository(context);
                var manufacturers = _fixture.CreateMany<Manufacturer>().ToList();
                var addedManufacturers = new List<Manufacturer>();

                // Act
                foreach (var manufacturer in manufacturers)
                {
                    var addedManufacturer = await repository.AddAsync(manufacturer);
                    addedManufacturers.Add(addedManufacturer);
                    addedManufacturer.Should().NotBeNull();
                    addedManufacturer.Id.Should().NotBe(Guid.Empty);
                }

                // Assert
                context.Manufacturers.Should().BeEquivalentTo(addedManufacturers);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingManufacturer_ReturnsUpdatedManufacturer()
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
                result.Should().BeEquivalentTo(updatedManufacturer);
                context.Manufacturers.Should().Contain(result);
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

        #endregion
    }
}
