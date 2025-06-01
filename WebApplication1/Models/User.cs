using Microsoft.AspNetCore.Mvc.ModelBinding; // Add this using statement
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public string? User_id { get; set; }

        [Required]
        [MaxLength(70)]
        public string? User_name { get; set; }

        [Required]
        [MaxLength(70)]
        public string? User_login { get; set; }

        [Required]
        [MaxLength(16)]
        public string? User_password { get; set; }
    }
}