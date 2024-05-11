using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    public class SignUpDto
    {
        [RegularExpression(@"^\S.*\S$",
            ErrorMessage = "The {0} must not start or end with a whitespace characters and must contain at least two letter.")]
        public string Name { get; set; } = "Unknown";
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Email must be in a proper format")]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailNotInUse", "Account", ErrorMessage = "This email address is already in use")]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
