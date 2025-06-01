using System.ComponentModel.DataAnnotations;

public class UserLoginDto
{
    [Required]
    public required string User_login { get; set; }

    [Required]
    public required string User_password { get; set; }
}