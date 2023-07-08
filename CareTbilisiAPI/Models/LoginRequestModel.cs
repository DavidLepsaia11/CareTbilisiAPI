using System.ComponentModel.DataAnnotations;

namespace CareTbilisiAPI.Models
{
    public class LoginRequestModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
