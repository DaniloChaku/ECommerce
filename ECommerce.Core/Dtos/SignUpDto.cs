using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user sign-up information.
    /// </summary>
    public class SignUpDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        [RegularExpression(@"^\S.*\S$",
            ErrorMessage = "The {0} must not start or end with whitespace characters and must contain at least two letters.")]
        public string Name { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress(ErrorMessage = "Email must be in a proper format")]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailNotInUse", "Account", ErrorMessage = "This email address is already in use")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirmation of the password.
        /// </summary>
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
