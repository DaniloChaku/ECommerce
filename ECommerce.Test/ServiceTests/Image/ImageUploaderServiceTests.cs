using ECommerce.Core.Exceptions;
using ECommerce.Core.Services.Image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Tests.ServiceTests.Image
{
    public class ImageUploaderServiceTests
    {
        private readonly ImageUploaderService _imageUploaderService;

        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;

        private readonly string _wwwrootPath = "wwwroot";
        private readonly string _productId = "123";
        private readonly string _productPath = "images/products/product-123";

        public ImageUploaderServiceTests()
        {
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(_wwwrootPath);

            _imageUploaderService = new ImageUploaderService(_mockWebHostEnvironment.Object);
        }

        [Fact]
        public async Task UploadImageAsync_WithNullImage_ThrowsImageUploadException()
        {
            // Arrange
            IFormFile? image = null;

            // Act
            Func<Task> act = async () => await _imageUploaderService.UploadImageAsync(image!, _productId);

            // Assert
            await act.Should().ThrowAsync<ImageUploadException>();
        }

        [Fact]
        public async Task UploadImageAsync_WithFileSizeExceedingLimit_ThrowsImageUploadException()
        {
            // Arrange
            var image = new Mock<IFormFile>();
            image.Setup(i => i.Length).Returns(ImageUploaderService.MaxFileSize + 1);

            // Act
            Func<Task> act = async () => await _imageUploaderService.UploadImageAsync(image.Object, _productId);

            // Assert
            await act.Should().ThrowAsync<ImageUploadException>();
        }

        [Fact]
        public async Task UploadImageAsync_WithInvalidFileType_ThrowsImageUploadException()
        {
            // Arrange
            var image = new Mock<IFormFile>();
            image.Setup(i => i.Length).Returns(1024);
            image.Setup(i => i.FileName).Returns("test.txt");

            // Act
            Func<Task> act = async () => await _imageUploaderService.UploadImageAsync(image.Object, _productId);

            // Assert
            await act.Should().ThrowAsync<ImageUploadException>();
        }

        [Fact]
        public async Task UploadImageAsync_WithValidImage_CreatesDirectoryAndReturnsImagePath()
        {
            // Arrange
            var image = new Mock<IFormFile>();
            image.Setup(i => i.Length).Returns(1024); 
            image.Setup(i => i.FileName).Returns("image.jpg");
            image.Setup(i => i.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _imageUploaderService.UploadImageAsync(image.Object, _productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().StartWith($"/{_productPath}/");
            _mockWebHostEnvironment.Verify(m => m.WebRootPath, Times.Once);
            Directory.Exists(Path.Combine(_wwwrootPath, _productPath)).Should().BeTrue();
        }
    }

}
