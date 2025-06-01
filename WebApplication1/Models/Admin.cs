public class Admin
{
    public int? Admin_id { get; set; } // Made nullable
    public required string Admin_login { get; set; }
    public required string Admin_password { get; set; }
}