using ECommerce.Core.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Test.RepositoryTests
{
    public class ProductRepositoryTest : IDisposable
    {
        private readonly IFixture _fixture;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public ProductRepositoryTest()
        {
            _fixture = new Fixture();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductRepositoryTests")
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
        public async Task GetAllAsync_EmptyDb_ReturnsAllProducts()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetAllAsync_NotEmptyDb_ReturnsAllProducts()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var products = _fixture.CreateMany<Product>().ToList();
                context.Products.AddRange(products);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEquivalentTo(products);
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
                var repository = new ProductRepository(context);

                // Act
                var result = await repository.GetByIdAsync(Guid.NewGuid());

                // Assert
                result.Should().Be(null);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsProduct()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _fixture.Create<Product>();
                context.Products.Add(product);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(product.Id);

                // Assert
                result.Should().BeEquivalentTo(product);
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
                var repository = new ProductRepository(context);
                var product = _fixture.Create<Product>();

                // Act
                var result = await repository.AddAsync(product);

                // Assert
                result.Should().BeTrue();
                context.Products.Should().Contain(product);
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var products = _fixture.CreateMany<Product>().ToList();

                // Act
                foreach (var Product in products)
                {
                    var result = await repository.AddAsync(Product);
                    result.Should().BeTrue();
                }

                // Assert
                context.Products.Should().BeEquivalentTo(products);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingProduct_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _fixture.Create<Product>();
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var updatedProduct = _fixture.Create<Product>();
                updatedProduct.Id = product.Id;

                // Act
                var result = await repository.UpdateAsync(updatedProduct);

                // Assert
                result.Should().BeTrue();
                context.Products.Should().Contain(updatedProduct);
            }
        }

        [Fact]
        public async Task UpdateAsync_NonExistentProduct_ReturnsFalse()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _fixture.Create<Product>();

                // Act
                var result = await repository.UpdateAsync(product);

                // Assert
                result.Should().BeFalse();
                context.Products.Should().NotContain(product);
            }
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ExistingProduct_RemovesProduct()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _fixture.Create<Product>();
                context.Products.Add(product);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.DeleteAsync(product);

                // Assert
                result.Should().BeTrue();
                context.Products.Should().NotContain(product);
            }
        }

        [Fact]
        public async Task DeleteAsync_NonExistentProduct_ReturnsFalse()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _fixture.Create<Product>();

                // Act
                var result = await repository.DeleteAsync(product);

                // Assert
                result.Should().BeFalse();
            }
        }

        #endregion
    }
}
