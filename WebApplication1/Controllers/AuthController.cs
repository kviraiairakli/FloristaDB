using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Data;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly WebApplication1DbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(WebApplication1DbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.User_login) || string.IsNullOrWhiteSpace(request.User_password))
            {
                return BadRequest("Login and password cannot be empty.");
            }

            var allUserIds = await _context.Users.Select(u => u.UserId).ToListAsync();
            var allAdminIds = await _context.Admins.Select(a => a.AdminId).ToListAsync();

            string newUserId;
            do
            {
                newUserId = Guid.NewGuid().ToString().Substring(0, 5);
            } while (allUserIds.Contains(newUserId) || allAdminIds.Contains(newUserId));

            if (await _context.Users.AnyAsync(u => u.UserLogin == request.User_login))
            {
                return Conflict("User with this login already exists.");
            }

            if (await _context.Admins.AnyAsync(a => a.AdminLogin == request.User_login))
            {
                return Conflict("Login is reserved by an administrator.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.User_password);

            var newUser = new User
            {
                UserId = newUserId,
                UserLogin = request.User_login,
                UserPassword = hashedPassword,
                UserName = "New User"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully!");
        }

        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.User_login) || string.IsNullOrWhiteSpace(request.User_password))
            {
                return BadRequest("Login and password cannot be empty.");
            }

            var allUserIds = await _context.Users.Select(u => u.UserId).ToListAsync();
            var allAdminIds = await _context.Admins.Select(a => a.AdminId).ToListAsync();

            string newAdminId;
            do
            {
                newAdminId = Guid.NewGuid().ToString().Substring(0, 5);
            } while (allUserIds.Contains(newAdminId) || allAdminIds.Contains(newAdminId));

            if (await _context.Admins.AnyAsync(a => a.AdminLogin == request.User_login))
            {
                return Conflict("Admin with this login already exists.");
            }

            if (await _context.Users.AnyAsync(u => u.UserLogin == request.User_login))
            {
                return Conflict("Login is reserved by a regular user.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.User_password);

            var newAdmin = new Admin
            {
                AdminId = newAdminId,
                AdminLogin = request.User_login,
                AdminPassword = hashedPassword,
                AdminUser = "New Admin",
                AdminLevel = "Basic"
            };

            _context.Admins.Add(newAdmin);
            await _context.SaveChangesAsync();

            return Ok("Admin registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.User_login) || string.IsNullOrWhiteSpace(request.User_password))
            {
                return BadRequest("Login and password cannot be empty.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserLogin == request.User_login);
            if (user != null && BCrypt.Net.BCrypt.Verify(request.User_password, user.UserPassword))
            {
                var token = GenerateJwtToken(user.UserId, "User");
                return Ok(new { Token = token, Role = "User", UserId = user.UserId });
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.AdminLogin == request.User_login);
            if (admin != null && BCrypt.Net.BCrypt.Verify(request.User_password, admin.AdminPassword))
            {
                var token = GenerateJwtToken(admin.AdminId, "Admin", admin.AdminLevel);
                return Ok(new { Token = token, Role = "Admin", AdminId = admin.AdminId, AdminLevel = admin.AdminLevel });
            }

            return Unauthorized("Invalid login or password.");
        }

        private string GenerateJwtToken(string id, string role, string adminLevel = "Super Admin")
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
            };

            if (role == "Admin" && !string.IsNullOrEmpty(adminLevel))
            {
                claims.Add(new Claim("AdminLevel", adminLevel));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}