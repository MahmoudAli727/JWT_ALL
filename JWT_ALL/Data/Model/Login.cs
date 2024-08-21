using System.ComponentModel.DataAnnotations;

namespace JWT_ALL.Data.Model
{
    public class Login
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
