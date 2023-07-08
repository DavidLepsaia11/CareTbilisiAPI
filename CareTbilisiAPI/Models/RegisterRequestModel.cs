using System.ComponentModel.DataAnnotations;

namespace CareTbilisiAPI.Models
{
    public class RegisterRequestModel
    {
        [Required , EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required , MaxLength(20, ErrorMessage ="User name max length is 20 characters"), MinLength(8, ErrorMessage = "User name min length is 8 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required, MaxLength(30, ErrorMessage = "Full name max length is 30 characters"), MinLength(8, ErrorMessage = "Full name min length is 6 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Compare(nameof(Password) , ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
