using ECommerce.Core.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Tests.Helpers;

namespace ECommerce.Test.RepositoryTests
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly IFixture _fixture;
        private readonly ProductCreationHelper _productCreationHelper;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public ProductRepositoryTests()
        {
            _fixture = new Fixture();
            _productCreationHelper = new ProductCreationHelper(_fixture);
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
                var products = _productCreationHelper.CreateManyProducts();
                context.Products.AddRange(products);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEquivalentTo(products);
            }
        }

        [Fact]
        public async Task GetAllAsync_WithFilter_ReturnsMatchingProducts()
        {

            using (var context = GetContext())
            {
                // Arrange
                var fixture = new Fixture();
                var repository = new ProductRepository(context);

                var product1 = _productCreationHelper.CreateProduct(false, "Bread");
                context.Products.Add(product1);

                var product2 = _productCreationHelper.CreateProduct(false, "Milk");
                context.Products.Add(product2);

                var product3 = _productCreationHelper.CreateProduct(false, "Candy");
                context.Products.Add(product3);

                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync(p => p.Name == "Candy");

                // Assert
                result.Should().ContainSingle(p => p.Id == product3.Id);
                result.Should().NotContain(p => p.Id == product2.Id);
                result.Should().NotContain(p => p.Id == product1.Id);
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
                var product = _productCreationHelper.CreateProduct(false);
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
        public async Task AddAsync_AddsEntityToDbSet_ReturnsAddedProduct()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _productCreationHelper.CreateProduct(false);

                // Act
                var result = await repository.AddAsync(product);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().NotBe(Guid.Empty);
                context.Products.Should().Contain(result);
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsAddedProducts()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var products = _productCreationHelper.CreateManyProducts();
                var addedProducts = new List<Product>();

                // Act
                foreach (var product in products)
                {
                    var addedProduct = await repository.AddAsync(product);
                    addedProducts.Add(addedProduct);
                    addedProduct.Should().NotBeNull();
                    addedProduct.Id.Should().NotBe(Guid.Empty);
                }

                // Assert
                context.Products.Should().BeEquivalentTo(addedProducts);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingProduct_ReturnsUpdatedProduct()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ProductRepository(context);
                var product = _productCreationHelper.CreateProduct(false);
                context.Products.Add(product);
                await context.SaveChangesAsync();

                var updatedProduct = _productCreationHelper.CreateProduct(false);
                updatedProduct.Id = product.Id;

                // Act
                var result = await repository.UpdateAsync(updatedProduct);

                // Assert
                result.Should().BeEquivalentTo(updatedProduct);
                context.Products.Should().Contain(updatedProduct);
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
                var product = _productCreationHelper.CreateProduct(false);
                context.Products.Add(product);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.DeleteAsync(product);

                // Assert
                result.Should().BeTrue();
                context.Products.Should().NotContain(product);
            }
        }

        #endregion
    }
}
