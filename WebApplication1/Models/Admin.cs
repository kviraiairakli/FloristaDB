using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Admins", Schema = "dbo")]
    public class Admin
    {
        public Admin()
        {
            AdminId = string.Empty;
            AdminUser = string.Empty;
            AdminLogin = string.Empty;
            AdminPassword = string.Empty;
            AdminLevel = string.Empty;
        }

        [Key]
        [Column("Admin_id")]
        [StringLength(5)]
        public string AdminId { get; set; }

        [Column("Admin_user")]
        [StringLength(30)]
        public string AdminUser { get; set; }

        [Required]
        [Column("Admin_login")]
        [StringLength(70)]
        public string AdminLogin { get; set; }

        [Required]
        [Column("Admin_password")]
        [StringLength(255)]
        public string AdminPassword { get; set; }

        [Column("Admin_level")]
        [StringLength(60)]
        public string AdminLevel { get; set; }
    }
}