using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    /// <summary>
    /// Represents a data transfer object (DTO) for user login credentials.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        [EmailAddress(ErrorMessage = "Email must be in a proper format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
