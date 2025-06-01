using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Data; // Added this line

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WebApplication1DbContext _context;

        public UsersController(WebApplication1DbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (_context.Users.Any(u => u.User_login == user.User_login))
            {
                return BadRequest(new { Message = "Email already exists." });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Registration successful!", UserId = user.User_id });
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UserLoginDto userLoginDto)
        {
            // Check for user in dbo.Users
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_login == userLoginDto.User_login);
            if (user != null)
            {
                if (user.User_password == userLoginDto.User_password)
                {
                    Debug.WriteLine($"User '{user.User_login}' logged in successfully.");
                    return Ok(new { Message = "User login successful!", UserId = user.User_id, user_role = "user" });
                }
                else
                {
                    Debug.WriteLine("User login attempt failed: Password mismatch.");
                    return Unauthorized(new { Message = "Invalid login attempt." });
                }
            }

            // If not found in users, check in dbo.Admins
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Admin_login == userLoginDto.User_login);
            if (admin != null)
            {
                if (admin.Admin_password == userLoginDto.User_password)
                {
                    Debug.WriteLine($"Admin '{admin.Admin_login}' logged in successfully.");
                    return Ok(new { Message = "Admin login successful!", AdminId = admin.Admin_id, user_role = "admin" });
                }
                else
                {
                    Debug.WriteLine("Admin login attempt failed: Admin password mismatch.");
                    return Unauthorized(new { Message = "Invalid login attempt." });
                }
            }

            // If not found in either table
            Debug.WriteLine($"Login attempt failed for '{userLoginDto.User_login}': User/Admin not found.");
            return Unauthorized(new { Message = "Invalid login attempt." });
        }
    }

    public class UserLoginDto
    {
        [Required]
        public required string User_login { get; set; }

        [Required]
        public required string User_password { get; set; }
    }
}