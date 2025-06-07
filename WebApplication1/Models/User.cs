using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Users", Schema = "dbo")]
    public class User
    {
        public User()
        {
            UserId = string.Empty;
            UserName = string.Empty;
            UserLogin = string.Empty;
            UserPassword = string.Empty;
        }

        [Key]
        [Column("User_id")]
        [StringLength(5)]
        public string UserId { get; set; }

        [Column("User_name")]
        [StringLength(30)]
        public string UserName { get; set; }

        [Required]
        [Column("User_login")]
        [StringLength(70)]
        public string UserLogin { get; set; }

        [Required]
        [Column("User_password")]
        [StringLength(255)]
        public string UserPassword { get; set; }
    }
}