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

        [Fact]
        public void Delete_WithNullImageUrl_DoesNotDeleteFile()
        {
            // Arrange
            var service = new ImageDeleterService(_mockWebHostEnvironment.Object);
            string? imageUrl = null;

            // Act
            service.Delete(imageUrl);

            // Assert
            Directory.GetFiles(_tempDirectory).Should().BeEmpty();
        }

        [Fact]
        public void Delete_WithNonExistentImage_DoesNotDeleteFile()
        {
            // Arrange
            var service = new ImageDeleterService(_mockWebHostEnvironment.Object);
            string imageUrl = "/images/product.jpg";

            // Act
            service.Delete(imageUrl);

            // Assert
            Directory.GetFiles(_tempDirectory).Should().BeEmpty();
        }

        [Fact]
        public void Delete_WithExistingImage_DeletesFile()
        {
            // Arrange
            var service = new ImageDeleterService(_mockWebHostEnvironment.Object);
            string imageUrl = "/images/product.jpg";
            string imagePath = Path.Combine(_tempDirectory, imageUrl.TrimStart('/'));
            string imageDirectory = Path.GetDirectoryName(imagePath)!;

            Directory.CreateDirectory(imageDirectory);

            File.WriteAllText(imagePath, "dummy content");

            // Act
            service.Delete(imageUrl);

            // Assert
            File.Exists(imagePath).Should().BeFalse();
        }

        public void Dispose()
        {
            Directory.Delete(_tempDirectory, true);
        }
    }
}
