namespace ECommerce.Core.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when an error occurs during image upload.
    /// </summary>
    public class ImageUploadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUploadException"/> class.
        /// </summary>
        public ImageUploadException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUploadException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ImageUploadException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageUploadException"/> class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ImageUploadException(string message, Exception innerException) : base(message, innerException) { }
    }
}
