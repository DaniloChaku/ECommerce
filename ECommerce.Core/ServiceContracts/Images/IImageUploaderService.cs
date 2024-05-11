using Microsoft.AspNetCore.Http;

namespace ECommerce.Core.ServiceContracts.Images
{
    /// <summary>
    /// Defines the contract for uploading images.
    /// </summary>
    public interface IImageUploaderService
    {
        /// <summary>
        /// Uploads an image asynchronously.
        /// </summary>
        /// <param name="image">The image file to upload.</param>
        /// <param name="productId">The identifier of the product associated with the image.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the URL of the uploaded image.</returns>
        Task<string> UploadAsync(IFormFile image, string productId);
    }
}
