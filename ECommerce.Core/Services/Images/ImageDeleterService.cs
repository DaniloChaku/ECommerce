using ECommerce.Core.ServiceContracts.Images;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.Core.Services.Images
{
    public class ImageDeleterService : IImageDeleterService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageDeleterService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public void DeleteImage(string? imageUrl)
        {
            if (imageUrl is not null)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string existingImagePath = Path.Combine(wwwrootPath, imageUrl.TrimStart('/'));
                if (File.Exists(existingImagePath))
                {
                    File.Delete(existingImagePath);
                }
            }
        }

        public void DeleteImageFolder(string productId)
        {
            string wwwrootPath = _webHostEnvironment.WebRootPath;
            string productPath = @"images/products/product-" + productId;
            string finalPath = Path.Combine(wwwrootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                Directory.Delete(finalPath, recursive: true);
            }
        }
    }
}
