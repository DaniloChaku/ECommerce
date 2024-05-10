using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using ECommerce.Core.ServiceContracts.Categories;
using ECommerce.Core.Services.Categories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.Test.ServiceTests
{
    public class CategoryServiceTests
    {
        private readonly ICategoryAdderService _categoryAdderService;
        private readonly ICategoryDeleterService _categoryDeleterService;
        private readonly ICategoryGetterService _categoryGetterService;
        private readonly ICategorySorterService _categorySorterService;
        private readonly ICategoryUpdaterService _categoryUpdaterService;

        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly ICategoryRepository _categoryRepository;

        private readonly IFixture _fixture;

        public CategoryServiceTests()
        {
            _fixture = new Fixture();

            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryRepository = _categoryRepositoryMock.Object;

            _categoryAdderService = new CategoryAdderService(_categoryRepository);
            _categoryDeleterService = new CategoryDeleterService(_categoryRepository);
            _categoryGetterService = new CategoryGetterService(_categoryRepository);
            _categorySorterService = new CategorySorterService();
            _categoryUpdaterService = new CategoryUpdaterService(_categoryRepository);
        }

        #region AddAsync

        [Fact]
        public async Task AddAsync_NullCategoryDto_ThrowsArgumentNullException()
        {
            // Arrange
            var categoryDto = null as CategoryDto;

            // Act
            var action = async () =>
            {
                await _categoryAdderService.AddAsync(categoryDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();    
        }

        [Fact]
        public async Task AddAsync_NonEmptyId_ThrowsArgumentException()
        {
            // Arrange
            var categoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, Guid.NewGuid())
                .Create();

            // Act
            var action = async () =>
            {
                await _categoryAdderService.AddAsync(categoryDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_DuplicateName_ThrowsArgumentException()
        {
            // Arrange
            var categoryDto1 = _fixture.Build<CategoryDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, "Test")
                .Create();
            var categoryDto2 = _fixture.Build<CategoryDto>()
                .With(t => t.Id, Guid.Empty)
                .With(t => t.Name, "Test")
                .Create();

            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>?>())).ReturnsAsync(new List<Category>() { categoryDto1.ToEntity() });

            // Act
            var action = async () =>
            {
                await _categoryAdderService.AddAsync(categoryDto2!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddAsync_ValidData_ReturnsAddedCategoryDto()
        {
            // Arrange
            var categoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, Guid.Empty).Create();
            var addedCategory = categoryDto.ToEntity();

            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Category, bool>>?>()))
                                   .ReturnsAsync(new List<Category>());
            _categoryRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(addedCategory);

            // Act
            var result = await _categoryAdderService.AddAsync(categoryDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(categoryDto); 
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_EmptyDb_ReturnsEmptyList()
        {
            // Arrange
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(new List<Category>());

            // Act
            var result = await _categoryGetterService.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_NonEmptyDb_ReturnsCategoryList()
        {
            // Arrange
            var categories = _fixture.CreateMany<Category>(3).ToList();
            _categoryRepositoryMock.Setup(repo => repo.GetAllAsync(null))
                                   .ReturnsAsync(categories);

            // Act
            var result = await _categoryGetterService.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(categories.Select(t => t.ToDto()));
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Category);

            // Act
            var result = await _categoryGetterService.GetByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCategoryDto()
        {
            // Arrange
            var category = _fixture.Create<Category>();
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(category.Id))
                                   .ReturnsAsync(category);

            // Act
            var result = await _categoryGetterService.GetByIdAsync(category.Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(category.ToDto());
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Category);

            // Act
            var action = async () =>
            {
                await _categoryDeleterService.DeleteAsync(invalidId);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task DeleteAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var existingCategory = _fixture.Build<Category>()
                .With(t => t.Id, categoryId).Create();

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
                                   .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<Category>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _categoryDeleterService.DeleteAsync(categoryId);

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_NullCategoryDto_ThrowsArgumentNullException()
        {
            // Arrange
            var categoryDto = null as CategoryDto;

            // Act
            var action = async () =>
            {
                await _categoryUpdaterService.UpdateAsync(categoryDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_EmptyId_ThrowsArgumentException()
        {
            // Arrange
            var categoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, Guid.Empty)
                .Create();

            // Act
            var action = async () =>
            {
                await _categoryUpdaterService.UpdateAsync(categoryDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.NewGuid(); 
            var categoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, invalidId)
                .Create();

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(invalidId))
                                   .ReturnsAsync(null as Category);

            // Act
            var action = async () =>
            {
                await _categoryUpdaterService.UpdateAsync(categoryDto!);
            };

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateAsync_ValidData_ReturnsUpdatedCategoryDto()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var existingCategory = _fixture.Build<Category>()
                .With(t => t.Id, categoryId)
                .Create();
            var updatedCategoryDto = _fixture.Build<CategoryDto>()
                .With(t => t.Id, categoryId)
                .Create();
            var updatedCategory = updatedCategoryDto.ToEntity();

            _categoryRepositoryMock.Setup(repo => repo.GetByIdAsync(categoryId))
                                   .ReturnsAsync(existingCategory);

            _categoryRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Category>()))
                                   .ReturnsAsync(updatedCategory);

            // Act
            var result = await _categoryUpdaterService.UpdateAsync(updatedCategoryDto);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updatedCategoryDto);
        }

        #endregion

        #region Sort

        [Fact]
        public void Sort_SortAscending_ReturnsSortedCategories()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryDto>();

            var sortedCategories = categories.OrderBy(t => t.Name);

            // Act
            var result = _categorySorterService.Sort(categories);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedCategories);
        }

        [Fact]
        public void Sort_SortDescending_ReturnsSortedCategories()
        {
            // Arrange
            var categories = _fixture.CreateMany<CategoryDto>();

            var sortedCategories = categories.OrderByDescending(t => t.Name);

            // Act
            var result = _categorySorterService.Sort(categories, SortOrder.DESC);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(sortedCategories);
        }

        [Fact]
        public void Sort_EmptyCategories_ReturnsEmptyList()
        {
            // Arrange
            var categories = new List<CategoryDto>();

            // Act
            var result = _categorySorterService.Sort(categories);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #endregion
    }
}
