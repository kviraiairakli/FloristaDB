using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserLoginDto
    {
        [Required]
        public string? Login { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}