using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Settings
{
    public class ImageUploadOptions
    {
        public const string Position = "ImageUpload";

        public long MaxImageSize { get; set; }
        public List<ExtensionMapping> AllowedTypes { get; set; } = [];
    }

    public class ExtensionMapping
    {
        public string Extension { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
    }
}
