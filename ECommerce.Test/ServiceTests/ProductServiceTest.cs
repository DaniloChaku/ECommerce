using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.DTO;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Product;
using ECommerce.Core.Services.Product;
using FluentAssertions.Common;
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
            _productSorterService = new ProductSorterService();
            _productUpdaterService = new ProductUpdaterService(_productRepository);
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_NullProductDto_ThrowsArgumentNullException()
        {
            // Arrange
            var productDto = null as ProductDto;

            // Act
            var actoin = async () =>
            {
                await _productAdderService.AddAsync(productDto!);
            };

            // Assert
            await actoin.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddAsync_NullName_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, null as string)
                .Create();

            // Act
            var actoin = async () =>
            {
                await _productAdderService.AddAsync(productDto);
            };

            // Assert
            await actoin.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.NewGuid())
                .Create();

            // Act
            var actoin = async () =>
            {
                await _productAdderService.AddAsync(productDto);
            };

            // Assert
            await actoin.Should().ThrowAsync<ArgumentException>();
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
            var actoin = async () =>
            {
                await _productAdderService.AddAsync(productDto2);
            };

            // Assert
            await actoin.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_NegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Price, -10).Create();

            // Act
            var actoin = async () =>
            {
                await _productAdderService.AddAsync(productDto);
            };

            // Assert
            await actoin.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsTrue()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty).Create();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>?>()))
                .ReturnsAsync(new List<Product>());
            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _productAdderService.AddAsync(productDto);

            // Assert
            result.Should().BeTrue();
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
        public async Task GetAllAsync_NonEmptyDb_ReturnsProductDtoList()
        {
            // Arrange
            var products = _fixture.CreateMany<Product>(3).ToList();
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

        #region GetByCategoryAsync

        [Fact]
        public async Task GetByCategoryAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            var categoryId = _fixture.Create<Guid>();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>?>()))
                                   .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productGetterService.GetByCategoryAsync(categoryId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByCategoryAsync_NonEmptyDb_ReturnsProductDtoList()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var products = _fixture.CreateMany<Product>().ToList();
            _productRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>?>()))
                .ReturnsAsync(products);
            var expected = products.Select(t => t.ToDto());

            // Act
            var result = await _productGetterService.GetByCategoryAsync(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region GetByManufacturerAsync

        [Fact]
        public async Task GetByManufacturerAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            var manufacturerId = _fixture.Create<Guid>();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>?>()))
                                   .ReturnsAsync(new List<Product>());

            // Act
            var result = await _productGetterService.GetByManufacturerAsync(manufacturerId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByManufacturerAsync_NonEmptyDb_ReturnsProductDtoList()
        {
            // Arrange
            var manufacturerId = Guid.NewGuid();
            var products = _fixture.CreateMany<Product>().ToList();
            _productRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>?>()))
                .ReturnsAsync(products);
            var expected = products.Select(t => t.ToDto());

            // Act
            var result = await _productGetterService.GetByManufacturerAsync(manufacturerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Product);

            // Act
            var action = async () =>
            {
                await _productDeleterService.DeleteAsync(invalidId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = _fixture.Build<Product>()
                .With(t => t.Id, productId).Create();

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                   .ReturnsAsync(existingProduct);

            _productRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Product>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _productDeleterService.DeleteAsync(productId);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_NullProductDto_ThrowsArgumentNullException()
        {
            // Arrange
            var productDto = null as ProductDto;

            // Act
            var action = async () =>
            {
                await _productUpdaterService.UpdateAsync(productDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_EmptyId_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();

            // Act
            var action = async () =>
            {
                await _productUpdaterService.UpdateAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_NullName_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, Guid.NewGuid())
                .With(t => t.Name, null as string)
                .Create();

            // Act
            var action = async () =>
            {
                await _productUpdaterService.UpdateAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Id, invalidId)
                .Create();

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Product);

            // Act
            var action = async () =>
            {
                await _productUpdaterService.UpdateAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_NegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _fixture.Build<ProductDto>()
                .With(t => t.Price, -2).Create();

            // Act
            var action = async () =>
            {
                await _productUpdaterService.UpdateAsync(productDto);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
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

        #region Sort

        [Fact]
        public void Sort_SortAscendingByName_ReturnsSortedCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            var sortedProducts = products.OrderBy(t => t.Name);

            // Act
            var result = _productSorterService.Sort(products, nameof(ProductDto.Name));

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public void Sort_SortDescendingByName_ReturnsSortedCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            var sortedProducts = products.OrderByDescending(t => t.Name);

            // Act
            var result = _productSorterService.Sort(products, nameof(ProductDto.Name), SortOrder.DESC);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public void Sort_SortAscendingByPrice_ReturnsSortedCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            var sortedProducts = products.OrderBy(t => t.Price);

            // Act
            var result = _productSorterService.Sort(products, nameof(ProductDto.Price));

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public void Sort_InvalidSortBy_ReturnsCategories()
        {
            // Arrange
            var products = _fixture.CreateMany<ProductDto>();

            // Act
            var result = _productSorterService.Sort(products, " ");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public void Sort_SortAscendingByNameOnSaleFirst_ReturnsSortedCategories()
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
            var result = _productSorterService.Sort(
                products, sortBy: nameof(ProductDto.Name), sortOrder: SortOrder.ASC, onSaleFirst: true);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedProducts);
        }

        [Fact]
        public void Sort_EmptyCategories_ReturnsEmptyList()
        {
            // Arrange
            var products = new List<ProductDto>();

            // Act
            var result = _productSorterService.Sort(products, "Name");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion
    }
}
