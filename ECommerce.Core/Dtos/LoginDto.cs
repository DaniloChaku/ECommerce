using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    public class LoginDto
    {
        [EmailAddress(ErrorMessage = "Email must be in a proper format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
