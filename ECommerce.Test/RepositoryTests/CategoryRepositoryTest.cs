using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Domain.RepositoryContracts;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Test.RepositoryTests
{
    public class CategoryRepositoryTest : IDisposable
    {
        private readonly IFixture _fixture;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public CategoryRepositoryTest()
        {
            _fixture = new Fixture();
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryRepositoryTests")
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
        public async Task GetAllAsync_EmptyDb_ReturnsAllCategories()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task GetAllAsync_NotEmptyDb_ReturnsAllCategories()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var categories = _fixture.CreateMany<Category>().ToList();
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                result.Should().BeEquivalentTo(categories);
            }
        }

        [Fact]
        public async Task GetAllAsync_WithFilter_ReturnsMatchingCategories()
        {
            
            using (var context = GetContext())
            {
                // Arrange
                var fixture = new Fixture();
                var repository = new CategoryRepository(context);

                var category1 = fixture.Build<Category>()
                    .With(c => c.Name, "Electronics").Create();
                context.Categories.Add(category1);

                var category2 = fixture.Build<Category>()
                    .With(c => c.Name, "Books").Create();
                context.Categories.Add(category2);

                var category3 = fixture.Build<Category>()
                    .With(c => c.Name, "Drinks").Create();
                context.Categories.Add(category3);

                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAllAsync(c => c.Name == "Electronics");

                // Assert
                result.Should().ContainSingle(c => c.Id == category1.Id);
                result.Should().NotContain(c => c.Id == category2.Id);
                result.Should().NotContain(c => c.Id == category3.Id);
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
                var repository = new CategoryRepository(context);

                // Act
                var result = await repository.GetByIdAsync(Guid.NewGuid());

                // Assert
                result.Should().Be(null);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCategory()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetByIdAsync(category.Id);

                // Assert
                result.Should().BeEquivalentTo(category);
            }
        }

        #endregion

        #region AddAsync

        [Fact]
        public async Task AddAsync_AddsEntityToDbSet_ReturnsAddedCategories()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();

                // Act
                var result = await repository.AddAsync(category);

                // Assert
                result.Should().NotBeNull();
                result.Id.Should().NotBe(Guid.Empty); 
                context.Categories.Should().Contain(result); 
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsAddedCategories()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var categories = _fixture.CreateMany<Category>().ToList();
                var addedCategories = new List<Category>();

                // Act
                foreach (var category in categories)
                {
                    var addedCategory = await repository.AddAsync(category);
                    addedCategories.Add(addedCategory);
                    addedCategory.Should().NotBeNull(); 
                    addedCategory.Id.Should().NotBe(Guid.Empty); 
                }

                // Assert
                context.Categories.Should().BeEquivalentTo(addedCategories);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingCategory_ReturnsUpdatedCategory()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                var updatedCategory = _fixture.Create<Category>();
                updatedCategory.Id = category.Id;

                // Act
                var result = await repository.UpdateAsync(updatedCategory);

                // Assert
                result.Should().BeEquivalentTo(updatedCategory);
                context.Categories.Should().Contain(updatedCategory);
            }
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ExistingCategory_RemovesCategory()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.DeleteAsync(category);

                // Assert
                result.Should().BeTrue();
                context.Categories.Should().NotContain(category);
            }
        }

        #endregion
    }
}