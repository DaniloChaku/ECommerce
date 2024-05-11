using ECommerce.Core.Domain.Entities;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Tests.Helpers;

namespace ECommerce.Tests.RepositoryTests
{
    public class ShoppingCartItemRepositoryTests : IDisposable
    {
        private readonly IFixture _fixture;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
        private readonly ShoppingCartItemCreationHelper _helper;

        public ShoppingCartItemRepositoryTests()
        {
            _fixture = new Fixture();
            _helper = new ShoppingCartItemCreationHelper(_fixture);

            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ShoppingCartItemRepositoryTests")
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
        public async Task GetAllAsync_EmptyDb_ReturnsAllShoppingCartItems()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetAllAsync_NotEmptyDb_ReturnsAllShoppingCartItems()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);
                var shoppingCartItems = _helper.CreateManyShoppingCartItems();
                context.ShoppingCartItems.AddRange(shoppingCartItems);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEquivalentTo(shoppingCartItems);
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
                var repository = new ShoppingCartItemRepository(context);

                // Act
                var result = await repository.GetByIdAsync(Guid.NewGuid());

                // Assert
                result.Should().Be(null);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsShoppingCartItem()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);
                var shoppingCartItem = _helper.CreateShoppingCartItem(false);
                context.ShoppingCartItems.Add(shoppingCartItem);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(shoppingCartItem.Id);

                // Assert
                result.Should().BeEquivalentTo(shoppingCartItem);
            }
        }

        #endregion

        #region AddAsync

        [Fact]
        public async Task AddAsync_AddsEntityToDbSet_ReturnsAddedShoppingCartItems()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);
                var shoppingcartitem = _helper.CreateShoppingCartItem();

                // Act
                var result = await repository.AddAsync(shoppingcartitem);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().NotBe(Guid.Empty);
                context.ShoppingCartItems.Should().Contain(result);
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsAddedShoppingCartItems()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);
                var shoppingcartitems = _helper.CreateManyShoppingCartItems();
                var addedShoppingCartItems = new List<ShoppingCartItem>();

                // Act
                foreach (var shoppingcartitem in shoppingcartitems)
                {
                    var addedShoppingCartItem = await repository.AddAsync(shoppingcartitem);
                    addedShoppingCartItems.Add(addedShoppingCartItem);
                    addedShoppingCartItem.Should().NotBeNull();
                    addedShoppingCartItem.Id.Should().NotBe(Guid.Empty);
                }

                // Assert
                context.ShoppingCartItems.Should().BeEquivalentTo(addedShoppingCartItems);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingShoppingCartItem_ReturnsUpdatedShoppingCartItem()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);
                var shoppingcartitem = _helper.CreateShoppingCartItem();
                context.ShoppingCartItems.Add(shoppingcartitem);
                await context.SaveChangesAsync();

                var updatedShoppingCartItem = _helper.CreateShoppingCartItem();
                updatedShoppingCartItem.Id = shoppingcartitem.Id;

                // Act
                var result = await repository.UpdateAsync(updatedShoppingCartItem);

                // Assert
                result.Should().BeEquivalentTo(updatedShoppingCartItem);
                context.ShoppingCartItems.Should().Contain(updatedShoppingCartItem);
            }
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ExistingShoppingCartItem_RemovesShoppingCartItem()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new ShoppingCartItemRepository(context);
                var shoppingcartitem = _helper.CreateShoppingCartItem();
                context.ShoppingCartItems.Add(shoppingcartitem);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.DeleteAsync(shoppingcartitem);

                // Assert
                result.Should().BeTrue();
                context.ShoppingCartItems.Should().NotContain(shoppingcartitem);
            }
        }

        #endregion
    }
}
