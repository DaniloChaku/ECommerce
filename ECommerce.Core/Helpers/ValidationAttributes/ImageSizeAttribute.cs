using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Helpers.ValidationAttributes
{
    /// <summary>
    /// Validates the size of an image uploaded via <see cref="IFormFile"/>.
    /// </summary>
    public class ImageSizeAttribute : ValidationAttribute
    {
        private readonly int _expectedWidth;
        private readonly int _expectedHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSizeAttribute"/> class
        /// with the specified expected width and height for the image.
        /// </summary>
        /// <param name="expectedWidth">The expected width of the image.</param>
        /// <param name="expectedHeight">The expected height of the image.</param>
        public ImageSizeAttribute(int expectedWidth, int expectedHeight)
        {
            _expectedWidth = expectedWidth;
            _expectedHeight = expectedHeight;
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>A <see cref="ValidationResult"/> object that represents the result of the validation.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile imageFile)
            {
                try
                {
                    using var image = Image.Load(imageFile.OpenReadStream());
                    if (image.Width != _expectedWidth || image.Height != _expectedHeight)
                    {
                        return new ValidationResult(string.Format(
                            ErrorMessage ?? "Image dimensions must be {0}x{1} pixels.",
                            _expectedWidth, _expectedHeight));
                    }
                }
                catch (UnknownImageFormatException)
                {
                    return new ValidationResult("The file is not an image");
                }
            }

            return ValidationResult.Success;
        }
    }
}
