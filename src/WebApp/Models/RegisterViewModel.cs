using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password has to be at least 6 characters")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The two password didn't match")]
        public string RePassword { get; set; }
    }
}
