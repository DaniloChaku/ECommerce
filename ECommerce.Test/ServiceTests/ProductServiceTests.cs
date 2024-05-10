using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Products;
using ECommerce.Core.Services.Products;
using ECommerce.Tests.Helpers;
using Moq;
using System.Linq.Expressions;

namespace ECommerce.Test.ServiceTests
{
    public class ProductServiceTests
    {
        private readonly IProductAdderService _productAdderService;
        private readonly IProductDeleterService _productDeleterService;
        private readonly IProductGetterService _productGetterService;
        private readonly IProductSorterService _productSorterService;
        private readonly IProductUpdaterService _productUpdaterService;

        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly IProductRepository _productRepository;

        private readonly IFixture _fixture;
        private readonly ProductCreationHelper _productCreationHelper;

        public ProductServiceTests()
        {
            _fixture = new Fixture();
            _productCreationHelper = new ProductCreationHelper(_fixture);

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
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var productDto = _productCreationHelper.CreateProductDto(false);

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
            var productDto1 = _productCreationHelper.CreateProductDto(true, "Test");
            var productDto2 = _productCreationHelper.CreateProductDto(true, "Test");

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>?>())).ReturnsAsync([productDto1.ToEntity()]);

            // Act
            var actoin = async () =>
            {
                await _productAdderService.AddAsync(productDto2);
            };

            // Assert
            await actoin.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsAddedProductDto()
        {
            // Arrange
            var productDto = _productCreationHelper.CreateProductDto();
            var addedProduct = productDto.ToEntity();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Product, bool>>?>()))
                .ReturnsAsync([]);
            _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(addedProduct);

            // Act
            var result = await _productAdderService.AddAsync(productDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(productDto);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync([]);

            // Act
            var result = await _productGetterService.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_NonEmptyDb_ReturnsProductDtoList()
        {
            // Arrange
            var products = _productCreationHelper.CreateManyProducts(5).ToList();
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(products);

            // Act
            var result = await _productGetterService.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(products.Select(p => p.ToDto()));
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
            var product = _productCreationHelper.CreateProduct(false);
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
                                   .ReturnsAsync([]);

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
            var products = _productCreationHelper.CreateManyProducts().ToList();
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>?>()))
                .ReturnsAsync(products);
            var expected = products.Select(p => p.ToDto());

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
                                   .ReturnsAsync([]);

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
            var products = _productCreationHelper.CreateManyProducts().ToList();
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>?>()))
                .ReturnsAsync(products);
            var expected = products.Select(p => p.ToDto());

            // Act
            var result = await _productGetterService.GetByManufacturerAsync(manufacturerId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        #endregion

        #region GetBySearchQueryAsync

        [Fact]
        public async Task GetBySearchQueryAsync_NullSearchQuery_ReturnsAllProducts()
        {
            // Arrange
            var products = _productCreationHelper.CreateManyProducts(5).ToList();
            var expectedProducts = products.Select(p => p.ToDto()).ToList();

            _productRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            // Act
            var result = await _productGetterService.GetBySearchQueryAsync(null);

            // Assert
            result.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetBySearchQueryAsync_ValidSearchQuery_ReturnsMatchingProducts()
        {
            // Arrange
            var searchQuery = "test";
            var products = new List<Product>
            {
                _productCreationHelper.CreateProduct(name: "Test Product 1"),
                _productCreationHelper.CreateProduct(name: "Another Product"),
                _productCreationHelper.CreateProduct(name: "Test Product 3"),
            };
            _productRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products.FindAll(p => p.Name.Contains(searchQuery, 
                StringComparison.CurrentCultureIgnoreCase)));

            // Act
            var result = await _productGetterService.GetBySearchQueryAsync(searchQuery);

            // Assert
            result.Should().OnlyContain(dto => dto.Name.Contains(searchQuery, 
                StringComparison.CurrentCultureIgnoreCase));
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
            var existingProduct = _productCreationHelper.CreateProduct();
            existingProduct.Id = productId;

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
            var productDto = _productCreationHelper.CreateProductDto();

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
            var productDto = _productCreationHelper.CreateProductDto();
            productDto.Id = invalidId;

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
        public async Task UpdateAsync_ValidData_ReturnsUpdatedProductDto()
        {
            // Arrange
            var productId = Guid.NewGuid();

            var existingProduct = _productCreationHelper.CreateProduct();
            existingProduct.Id = productId;

            var updatedProductDto = _productCreationHelper.CreateProductDto();
            updatedProductDto.Id = productId;
            var updatedProduct = updatedProductDto.ToEntity();

            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId))
                                   .ReturnsAsync(existingProduct);

            _productRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                                   .ReturnsAsync(updatedProduct);

            // Act
            var result = await _productUpdaterService.UpdateAsync(updatedProductDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedProductDto);
        }

        #endregion

        #region Sort

        [Fact]
        public void Sort_SortAscendingByName_ReturnsSortedCategories()
        {
            // Arrange
            var products = _productCreationHelper.CreateManyProductDtos();

            var sortedProducts = products.OrderBy(p => p.Name);

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
            var products = _productCreationHelper.CreateManyProductDtos();

            var sortedProducts = products.OrderByDescending(p => p.Name);

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
            var products = _productCreationHelper.CreateManyProductDtos();

            var sortedProducts = products.OrderBy(p => p.Price);

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
            var products = _productCreationHelper.CreateManyProductDtos();

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
                _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create(),

                _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, null as decimal?)
                .With(t => t.Stock, 10)
                .Create(),

                _fixture.Build<ProductDto>()
                .With(t => t.Price, 10)
                .With(t => t.SalePrice, 5)
                .With(t => t.Stock, 10).Create(),
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
