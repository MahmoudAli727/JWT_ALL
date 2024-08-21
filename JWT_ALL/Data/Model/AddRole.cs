using System.ComponentModel.DataAnnotations;

namespace JWT_ALL.Data.Model
{
    public class AddRole
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
