using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        public string? Password { get; set; }
    }
}
