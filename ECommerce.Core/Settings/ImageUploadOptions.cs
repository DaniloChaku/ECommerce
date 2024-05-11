namespace ECommerce.Core.Settings
{
    /// <summary>
    /// Options for image upload settings.
    /// </summary>
    public class ImageUploadOptions
    {
        /// <summary>
        /// Configuration position for image upload settings.
        /// </summary>
        public const string Position = "ImageUpload";

        /// <summary>
        /// Maximum allowed size for uploaded images.
        /// </summary>
        public long MaxImageSize { get; set; }

        /// <summary>
        /// List of allowed image types and their corresponding extensions.
        /// </summary>
        public List<ExtensionMapping> AllowedTypes { get; set; } = [];
    }

    /// <summary>
    /// Represents the mapping between file extension and content type.
    /// </summary>
    public class ExtensionMapping
    {
        /// <summary>
        /// File extension.
        /// </summary>
        public string Extension { get; set; } = string.Empty;

        /// <summary>
        /// Content type associated with the file extension.
        /// </summary>
        public string ContentType { get; set; } = string.Empty;
    }
}
