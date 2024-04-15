using ECommerce.Core.Exceptions;
using ECommerce.Core.ServiceContracts.Image;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Image
{
    public class ImageUploaderService : IImageUploaderService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public const long MaxFileSize = 10 * 1024 * 1024; 
        public static string[] PermittedExtensions { get; } = { ".jpg", ".jpeg", ".png", ".gif" };

        public ImageUploaderService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadImageAsync(IFormFile image, string productId)
        {
            if (image is null)
            {
                throw new ImageUploadException("Image was not supplied");
            }

            if (image.Length > MaxFileSize)
            {
                throw new ImageUploadException($"File size exceeds the maximum allowed limit" +
                    $" of {MaxFileSize / 1024 / 1024} MB.");
            }

            var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !PermittedExtensions.Contains(ext))
            {
                throw new ImageUploadException("Invalid file type. Only image files are allowed " +
                    $"({string.Join(", ", PermittedExtensions)}).");
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
