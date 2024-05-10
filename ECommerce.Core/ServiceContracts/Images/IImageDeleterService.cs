using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.ServiceContracts.Images
{
    public interface IImageDeleterService
    {
        void DeleteImage(string? imageUrl);
        void DeleteImageFolder(string productId);
    }
}
