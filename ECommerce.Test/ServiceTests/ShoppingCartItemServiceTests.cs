using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.ServiceContracts.ShoppingCartItems;
using ECommerce.Core.Services.ShoppingCartItem;
using ECommerce.Tests.Helpers;
using Moq;
using System.Linq.Expressions;

namespace ECommerce.Test.ServiceTests
{
    public class ShoppingCartItemServiceTests
    {
        private readonly IShoppingCartItemAdderService _shoppingCartItemAdderService;
        private readonly IShoppingCartItemDeleterService _shoppingCartItemDeleterService;
        private readonly IShoppingCartItemGetterService _shoppingCartItemGetterService;
        private readonly IShoppingCartItemUpdaterService _shoppingCartItemUpdaterService;
        private readonly IProductGetterService _productGetterService;

        private readonly Mock<IShoppingCartItemRepository> _shoppingCartItemRepositoryMock;
        private readonly Mock<IProductGetterService> _productGetterServiceMock; 
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        private readonly IFixture _fixture;
        private readonly ShoppingCartItemCreationHelper _helper;

        public ShoppingCartItemServiceTests()
        {
            _fixture = new Fixture();
            _helper = new ShoppingCartItemCreationHelper(_fixture);

            _shoppingCartItemRepositoryMock = new Mock<IShoppingCartItemRepository>();
            _shoppingCartItemRepository = _shoppingCartItemRepositoryMock.Object;

            _productGetterServiceMock = new Mock<IProductGetterService>();
            _productGetterService = _productGetterServiceMock.Object;

            _shoppingCartItemAdderService = new ShoppingCartItemAdderService(_shoppingCartItemRepository,
                _productGetterService);
            _shoppingCartItemDeleterService = new ShoppingCartItemDeleterService(_shoppingCartItemRepository);
            _shoppingCartItemGetterService = new ShoppingCartItemGetterService(_shoppingCartItemRepository);
            _shoppingCartItemUpdaterService = new ShoppingCartItemUpdaterService(_shoppingCartItemRepository,
                _productGetterService);
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_NullShoppingCartItemDto_ThrowsArgumentNullException()
        {
            // Arrange
            var shoppingCartItemDto = null as ShoppingCartItemDto;

            // Act
            var action = async () =>
            {
                await _shoppingCartItemAdderService.AddAsync(shoppingCartItemDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, Guid.NewGuid())
                .Create();

            // Act
            var action = async () =>
            {
                await _shoppingCartItemAdderService.AddAsync(shoppingCartItemDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_InvalidProductId_ThrowsArgumentException()
        {
            // Arrange
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();
            var expectedProductDto = null as ProductDto;

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProductDto);

            // Act
            var action = async () =>
            {
                await _shoppingCartItemAdderService.AddAsync(shoppingCartItemDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_CountGreaterThanProductStock_ThrowsQuantityExceedsStockException()
        {
            // Arrange
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Count, 10)
                .Create();
            var expectedProductDto = new ProductDto() { Stock = 5 };

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProductDto);

            // Act
            var action = async () =>
            {
                await _shoppingCartItemAdderService.AddAsync(shoppingCartItemDto);
            };

            // Assert
            await action.Should().ThrowAsync<QuantityExceedsStockException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsAddedShoppingCartItemDto()
        {
            // Arrange
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Count, 5)
                .With(t => t.ProductName, null as string)
                .With(t => t.ProductPrice, null as decimal?)
                .With(t => t.ProductPriceType, null as PriceType?)
                .Create();
            var addedShoppingCartItem = shoppingCartItemDto.ToEntity();
            var expectedProductDto = new ProductDto() { Stock = 10 };

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProductDto);
            _shoppingCartItemRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ShoppingCartItem>()))
                .ReturnsAsync(addedShoppingCartItem);

            // Act
            var result = await _shoppingCartItemAdderService.AddAsync(shoppingCartItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(shoppingCartItemDto);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(new List<ShoppingCartItem>());

            // Act
            var result = await _shoppingCartItemGetterService.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_NonEmptyDb_ReturnsShoppingCartItemList()
        {
            // Arrange
            var shoppingcartitems = _helper.CreateManyShoppingCartItems();
            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(shoppingcartitems);

            // Act
            var result = await _shoppingCartItemGetterService.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(shoppingcartitems.Select(t => t.ToDto()));
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as ShoppingCartItem);

            // Act
            var result = await _shoppingCartItemGetterService.GetByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsShoppingCartItemDto()
        {
            // Arrange
            var shoppingCartItem = _helper.CreateShoppingCartItem();
            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(shoppingCartItem.Id))
                                   .ReturnsAsync(shoppingCartItem);

            // Act
            var result = await _shoppingCartItemGetterService.GetByIdAsync(shoppingCartItem.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(shoppingCartItem.ToDto());
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as ShoppingCartItem);

            // Act
            var action = async () =>
            {
                await _shoppingCartItemDeleterService.DeleteAsync(invalidId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var existingShoppingCartItem = _helper.CreateShoppingCartItem();

            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                                   .ReturnsAsync(existingShoppingCartItem);

            _shoppingCartItemRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<ShoppingCartItem>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _shoppingCartItemDeleterService.DeleteAsync(existingShoppingCartItem.Id);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_NullShoppingCartItemDto_ThrowsArgumentNullException()
        {
            // Arrange
            var shoppingCartItemDto = null as ShoppingCartItemDto;

            // Act
            var action = async () =>
            {
                await _shoppingCartItemUpdaterService.UpdateAsync(shoppingCartItemDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_EmptyId_ThrowsArgumentException()
        {
            // Arrange
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();

            // Act
            var action = async () =>
            {
                await _shoppingCartItemUpdaterService.UpdateAsync(shoppingCartItemDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, invalidId)
                .Create();

            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as ShoppingCartItem);

            // Act
            var action = async () =>
            {
                await _shoppingCartItemUpdaterService.UpdateAsync(shoppingCartItemDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidProductId_ThrowsArgumentException()
        {
            // Arrange
            var shoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();
            var expectedProductDto = null as ProductDto;

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProductDto);

            // Act
            var action = async () =>
            {
                await _shoppingCartItemUpdaterService.UpdateAsync(shoppingCartItemDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_CountGreaterThanProductStock_ThrowsQuantityExceedsStockException()
        {
            // Arrange
            var existingShoppingCartItem = _helper.CreateShoppingCartItem(false);
            var shoppingCartItemId = existingShoppingCartItem.Id;
            var updatedShoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, shoppingCartItemId)
                .Create();
            var expectedProductDto = new ProductDto() { Stock = 5 };

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProductDto);

            // Act
            var action = async () =>
            {
                await _shoppingCartItemUpdaterService.UpdateAsync(updatedShoppingCartItemDto);
            };

            // Assert
            await action.Should().ThrowAsync<QuantityExceedsStockException>();
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ReturnsUpdatedShoppingCartItemDto()
        {
            // Arrange
            var existingShoppingCartItem = _helper.CreateShoppingCartItem(false);
            var updatedShoppingCartItemDto = _fixture.Build<ShoppingCartItemDto>()
                .With(t => t.Id, existingShoppingCartItem.Id)
                .With(t => t.Count, 5)
                .With(t => t.ProductName, null as string)
                .With(t => t.ProductPrice, null as decimal?)
                .With(t => t.ProductPriceType, null as PriceType?)
                .Create();
            var updatedShoppingCartItem = updatedShoppingCartItemDto.ToEntity();
            var expectedProductDto = new ProductDto() { Stock = 10 };
            
            _shoppingCartItemRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingShoppingCartItem);

            _shoppingCartItemRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<ShoppingCartItem>()))
                .ReturnsAsync(updatedShoppingCartItem);

            _productGetterServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedProductDto);

            // Act
            var result = await _shoppingCartItemUpdaterService.UpdateAsync(updatedShoppingCartItemDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedShoppingCartItemDto);
        }

        #endregion
    }
}
