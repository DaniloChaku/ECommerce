using ECommerce.Core.Exceptions;
using ECommerce.Core.Services.Images;
using ECommerce.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
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

        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private readonly Mock<IOptions<ImageUploadOptions>> _imageUploadOptionsMock;
        private readonly ImageUploadOptions _imageUploadOptions;

        private readonly string _wwwrootPath = "wwwroot";
        private readonly string _productId = "123";
        private readonly string _productPath = "images/products/product-123";

        public ImageUploaderServiceTests()
        {
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _webHostEnvironmentMock.Setup(m => m.WebRootPath).Returns(_wwwrootPath);

            _imageUploadOptions = new ImageUploadOptions()
            {
                AllowedTypes = new List<ExtensionMapping>()
                {
                    new ExtensionMapping()
                    {
                        Extension = ".jpg",
                        ContentType = "image/jpeg"
                    },
                    new ExtensionMapping()
                    {
                        Extension = ".jpeg",
                        ContentType = "image/jpeg"
                    },
                    new ExtensionMapping()
                    {
                        Extension = ".png",
                        ContentType = "image/png"
                    },
                    new ExtensionMapping()
                    {
                        Extension = ".gif",
                        ContentType = "image/gif"
                    }
                },
                MaxImageSize = 1024 * 1024 * 10,
            };

            _imageUploadOptionsMock = new Mock<IOptions<ImageUploadOptions>>();
            _imageUploadOptionsMock.Setup(m => m.Value).Returns(_imageUploadOptions);

            _imageUploaderService = new ImageUploaderService(_webHostEnvironmentMock.Object,
                _imageUploadOptionsMock.Object);
        }

        [Fact]
        public async Task UploadAsync_WithNullImage_ThrowsImageUploadException()
        {
            // Arrange
            IFormFile? image = null;

            // Act
            Func<Task> act = async () => await _imageUploaderService.UploadAsync(image!, _productId);

            // Assert
            await act.Should().ThrowAsync<ImageUploadException>();
        }

        [Fact]
        public async Task UploadAsync_WithFileSizeExceedingLimit_ThrowsImageUploadException()
        {
            // Arrange
            var image = new Mock<IFormFile>();
            image.Setup(i => i.Length).Returns(_imageUploadOptions.MaxImageSize + 1);

            // Act
            Func<Task> act = async () => await _imageUploaderService.UploadAsync(image.Object, _productId);

            // Assert
            await act.Should().ThrowAsync<ImageUploadException>();
        }

        [Fact]
        public async Task UploadAsync_WithInvalidFileType_ThrowsImageUploadException()
        {
            // Arrange
            var image = new Mock<IFormFile>();
            image.Setup(i => i.Length).Returns(1024);
            image.Setup(i => i.FileName).Returns("test.txt");

            // Act
            Func<Task> act = async () => await _imageUploaderService.UploadAsync(image.Object, _productId);

            // Assert
            await act.Should().ThrowAsync<ImageUploadException>();
        }

        [Fact]
        public async Task UploadAsync_WithValidImage_CreatesDirectoryAndReturnsImagePath()
        {
            // Arrange
            var image = new Mock<IFormFile>();
            image.Setup(i => i.Length).Returns(1024); 
            image.Setup(i => i.FileName).Returns("image.jpg");
            image.Setup(i => i.ContentType).Returns("image/jpeg");
            image.Setup(i => i.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _imageUploaderService.UploadAsync(image.Object, _productId);

            // Assert
            result.Should().NotBeNull();
            result.Should().StartWith($"/{_productPath}/");
            _webHostEnvironmentMock.Verify(m => m.WebRootPath, Times.Once);
            Directory.Exists(Path.Combine(_wwwrootPath, _productPath)).Should().BeTrue();
        }
    }

}
