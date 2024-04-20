using ECommerce.Core.Domain.Entities;
using ECommerce.Core.Services.Image;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.ServiceTests.Image
{
    public class ImageDeleterServiceTests : IDisposable
    {
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;
        private readonly string _tempDirectory;

        public ImageDeleterServiceTests()
        {
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(_tempDirectory);
            _mockWebHostEnvironment.Setup(e => e.WebRootPath).Returns(_tempDirectory);
        }

        private ImageDeleterService CreateImageDeleterService()
        {
            return new ImageDeleterService(_mockWebHostEnvironment.Object);
        }

        private void CreateDirectoryWithImage(string imagePath, string? directory = null)
        {
            if (directory == null)
            {
                directory = Path.GetDirectoryName(imagePath)!;
            }
            Directory.CreateDirectory(directory);

            File.WriteAllText(imagePath, "dummy content");
        }

        [Fact]
        public void DeleteImage_WithNullImageUrl_DoesNotDeleteFile()
        {
            // Arrange
            var service = CreateImageDeleterService();
            string? imageUrl = null;

            // Act
            service.DeleteImage(imageUrl);

            // Assert
            Directory.GetFiles(_tempDirectory).Should().BeEmpty();
        }

        [Fact]
        public void DeleteImage_WithExistingImage_DeletesFile()
        {
            // Arrange
            var service = CreateImageDeleterService();
            string imageUrl = "/images/product.jpg";
            string imagePath = Path.Combine(_tempDirectory, imageUrl.TrimStart('/'));
            CreateDirectoryWithImage(imagePath);

            // Act
            service.DeleteImage(imageUrl);

            // Assert
            File.Exists(imagePath).Should().BeFalse();
        }

        [Fact]
        public void DeleteImageFolder_WithExistingFolder_DeletesFolderRecursively()
        {
            // Arrange
            var service = CreateImageDeleterService();
            var productId = "1";
            string imageUrl = $"images/products/product-{productId}/image.jpg";
            string imagePath = Path.Combine(_tempDirectory, imageUrl);
            string imageDirectory = Path.GetDirectoryName(imagePath)!;

            CreateDirectoryWithImage(imagePath, imageDirectory);

            // Act
            service.DeleteImageFolder(productId);

            // Assert
            Directory.Exists(imageDirectory).Should().BeFalse();
        }

        public void Dispose()
        {
            Directory.Delete(_tempDirectory, true);
        }
    }
}
