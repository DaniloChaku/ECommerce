using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Images;
using ECommerce.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ECommerce.Core.Services.Images
{
    public class ImageUploaderService : IImageUploaderService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ImageUploadOptions _imageUploadSettings;

        public ImageUploaderService(IWebHostEnvironment webHostEnvironment,
            IOptions<ImageUploadOptions> imageUploadSettings)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageUploadSettings = imageUploadSettings.Value;
        }

        public async Task<string> UploadAsync(IFormFile image, string productId)
        {
            if (image is null)
            {
                throw new ImageUploadException("Image was not supplied");
            }

            var maxImageSize = _imageUploadSettings.MaxImageSize;
            if (image.Length > maxImageSize)
            {
                throw new ImageUploadException($"File size exceeds the maximum allowed limit" +
                    $" of {maxImageSize / 1024 / 1024} MB.");
            }

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            var contentType = image.ContentType;

            var allowedTypes = _imageUploadSettings.AllowedTypes;
            var hasValidContentType = allowedTypes
                .Any(t => t.Extension.Equals(ext) && t.ContentType.Equals(contentType));

            if (!hasValidContentType)
            {
                throw new ImageUploadException("Invalid file type. Only image files are allowed " +
                    $"({string.Join(", ", allowedTypes.Select(e => e.Extension))}).");
            }

            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string productPath = @"images/products/product-" + productId;
            string finalPath = Path.Combine(wwwrootPath, productPath);

            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }

            using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return $"/{productPath}/{fileName}";
        }
    }
}
