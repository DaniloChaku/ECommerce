using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Image
{
    public interface IImageUploaderService
    {
        Task<string> UploadImageAsync(IFormFile image, string productId);
    }
}
