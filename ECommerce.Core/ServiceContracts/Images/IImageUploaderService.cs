using Microsoft.AspNetCore.Http;

namespace ECommerce.Core.ServiceContracts.Images
{
    public interface IImageUploaderService
    {
        Task<string> UploadAsync(IFormFile image, string productId);
    }
}
