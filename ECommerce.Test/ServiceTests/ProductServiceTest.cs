using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.Services.Product;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Test.ServiceTests
{
    public class ProductServiceTest
    {
        private readonly IProductAdderService _productAdderService;
        private readonly IProductDeleterService _productDeleterService;
        private readonly IProductGetterService _productGetterService;
        private readonly IProductSorterService _productSorterService;
        private readonly IProductUpdaterService _productUpdaterService;

        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IProductRepository _productRepository;

        private readonly IFixture _fixture;

        public ProductServiceTest()
        {
            _fixture = new Fixture();

            _productRepositoryMock = new Mock<IProductRepository>();
            _productRepository = _productRepositoryMock.Object;

            _productAdderService = new ProductAdderService(_productRepository);
            _productDeleterService = new ProductDeleterService(_productRepository);
            _productGetterService = new ProductGetterService(_productRepository);
            _productSorterService = new ProductSorterService(_productRepository);
            _productUpdaterService = new ProductUpdaterService(_productRepository);
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_NullName_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, null as string)
                .Create();

            // Act
            var action = async () =>
            {
                await _productAdderService.AddAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.NewGuid())
                .Create();

            // Act
            var action = async () =>
            {
                await _productAdderService.AddAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_DuplicateName_ThrowsArgumentException()
        {
            // Arrange
            var productDto1 = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, "Test")
                .Create();
            var productDto2 = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, "Test")
                .Create();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>?>())).ReturnsAsync(new List<Product>() { productDto1.ToEntity() });

            // Act
            var action = async () =>
            {
                await _productAdderService.AddAsync(productDto2);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_NegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Price, -10).Create();

            // Act
            var action = async () =>
            {
                await _productAdderService.AddAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsProductDto()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty).Create();
            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _productAdderService.AddAsync(productDto);

            productDto.Id = result.Id;

            // Assert
            result.Id.Should().NotBe(Guid.Empty);
            result.Should().BeEquivalentTo(productDto);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productGetterService.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_NonEmptyDb_ReturnsProductList()
        {
            // Arrange
            var products = _fixture.CreateMany<Product>(3);
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(products);

            // Act
            var result = await _productGetterService.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(products.Select(t => t.ToDto()));
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Product);

            // Act
            var result = await _productGetterService.GetByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsProductDto()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(product.Id))
                                   .ReturnsAsync(product);

            // Act
            var result = await _productGetterService.GetByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product.ToDto());
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                   .ReturnsAsync(new Product { Id = productId });

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Product>()))
                                   .ReturnsAsync(true);

            // Ac
            var result = await _productDeleterService.DeleteAsync(productId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Product);

            // Act
            var result = await _productDeleterService.DeleteAsync(invalidId);

            // Assert
            result.Should().BeFalse();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_EmptyId_ReturnsFalse()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();

            // Act
            var result = await _productUpdaterService.UpdateAsync(productDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_NullName_ReturnsFalse()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.NewGuid())
                .With(t => t.Name, null as string)
                .Create();

            // Act
            var result = await _productUpdaterService.UpdateAsync(productDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ReturnsFalse()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, invalidId)
                .Create();

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Product);

            // Act
            var result = await _productUpdaterService.UpdateAsync(productDto);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_NegativePrice_ReturnsFalse()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Price, -2).Create();

            // Act
            var result = await _productUpdaterService.UpdateAsync(productDto);

            // Assert
            result.Should().BeFalse();
        }

            [Fact]
        public async Task UpdateAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = _fixture.Build<Product>()
                .With(t => t.Id, productId)
                .Create();
            var updatedProductDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, productId)
                .Create();

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                   .ReturnsAsync(existingProduct);

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _productUpdaterService.UpdateAsync(updatedProductDto);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region SortAsync

        [Fact]
        public async Task SortAsync_SortAscendingByName_ReturnsSortedCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            var sortedProducts = products.OrderBy(t => t.Name);

            // Act
            var result = await _productSorterService.SortAsync(products, nameof(ProductDto.Name));

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public async Task SortAsync_SortDescendingByName_ReturnsSortedCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            var sortedProducts = products.OrderByDescending(t => t.Name);

            // Act
            var result = await _productSorterService.SortAsync(products, nameof(ProductDto.Name), SortOrder.DESC);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public async Task SortAsync_SortAscendingByPrice_ReturnsSortedCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            var sortedProducts = products.OrderBy(t => t.Price);

            // Act
            var result = await _productSorterService.SortAsync(products, nameof(ProductDto.Price));

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public async Task SortAsync_InvalidSortBy_ReturnsCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            // Act
            var result = await _productSorterService.SortAsync(products, " ");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task SortAsync_SortAscendingByNameOnSaleFirst_ReturnsSortedCategories()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                _fixture.Create<ProductDto>(),
                _fixture.Create<ProductDto>(),
                _fixture.Build<ProductDto>().With(t => t.SalePrice, null as decimal?).Create(),
            };

            var sortedProducts = products.OrderBy(p => p.SalePrice.HasValue ? 0 : 1).ThenBy(p => p.Name);

            // Act
            var result = await _productSorterService
                .SortAsync(products, sortBy: nameof(ProductDto.Name), sortOrder: SortOrder.ASC, onSaleFirst: true);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public async Task SortAsync_EmptyCategories_ReturnsEmptyList()
        {
            // Arrange
            var products = new List<ProductDto>();

            // Act
            var result = await _productSorterService.SortAsync(products, "Name");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion
    }
}
