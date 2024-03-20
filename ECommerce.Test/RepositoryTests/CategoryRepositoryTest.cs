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
        public async Task AddAsync_AddsEntityToDbSet_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();

                // Act
                var result = await repository.AddAsync(category);

                // Assert
                result.Should().BeTrue();
                context.Categories.Should().Contain(category);
            }
        }

        [Fact]
        public async Task AddAsync_AddsMultipleEntitiesToDbSet_ReturnsTrue()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var categories = _fixture.CreateMany<Category>().ToList();

                // Act
                foreach (var category in categories)
                {
                    var result = await repository.AddAsync(category);
                    result.Should().BeTrue();
                }

                // Assert
                context.Categories.Should().BeEquivalentTo(categories);
            }
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ExistingCategory_ReturnsTrue()
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
                result.Should().BeTrue();
                context.Categories.Should().Contain(updatedCategory);
            }
        }

        [Fact]
        public async Task UpdateAsync_NonExistentCategory_ReturnsFalse()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();

                // Act
                var result = await repository.UpdateAsync(category);

                // Assert
                result.Should().BeFalse();
                context.Categories.Should().NotContain(category);
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

        [Fact]
        public async Task DeleteAsync_NonExistentCategory_ReturnsFalse()
        {
            using (var context = GetContext())
            {
                // Arrange
                var repository = new CategoryRepository(context);
                var category = _fixture.Create<Category>();

                // Act
                var result = await repository.DeleteAsync(category);

                // Assert
                result.Should().BeFalse();
            }
        }

        #endregion
    }
}