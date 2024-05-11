using ECommerce.Core.ServiceContracts.Images;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.Core.Services.Images
{
    /// <summary>
    /// Service for deleting images.
    /// </summary>
    public class ImageDeleterService : IImageDeleterService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageDeleterService"/> class.
        /// </summary>
        /// <param name="webHostEnvironment">The hosting environment.</param>
        public ImageDeleterService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Deletes the specified image.
        /// </summary>
        /// <param name="imageUrl">The URL of the image to delete.</param>
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

        /// <summary>
        /// Deletes the folder containing images for a specified product.
        /// </summary>
        /// <param name="productId">The ID of the product whose image folder is to be deleted.</param>
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
