using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Helpers.ValidationAttributes
{
    public class ImageSizeAttribute : ValidationAttribute
    {
        private readonly int _expectedWidth;
        private readonly int _expectedHeight;

        public ImageSizeAttribute(int expectedWidth, int expectedHeight)
        {
            _expectedWidth = expectedWidth;
            _expectedHeight = expectedHeight;
        }

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