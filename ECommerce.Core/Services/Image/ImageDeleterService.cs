using ECommerce.Core.ServiceContracts.Image;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Services.Image
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
