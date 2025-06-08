using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class UserLoginDto
    {
        public string User_login { get; set; } // Matches the JSON field name
        public string User_password { get; set; } // Matches the JSON field name
    }
}