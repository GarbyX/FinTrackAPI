using Microsoft.AspNetCore.Mvc;
using FinTrackAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrackAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        private readonly AppDbContext _context; // Declare _context

        // Inject AppDbContext via the constructor
        public AuthController(AppDbContext context)
        {
            _context = context; // Assign the injected context to _context
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            // Use _context to query the database
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Password != password)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok("Login successful.");
            
            // Mock authentication for demo purposes
            // if (login.Username == "test" && login.PasswordHash == "password")
            // {
            //     var token = GenerateJwtToken(login.Username);
            //     return Ok(new { token });
            // }
            // return Unauthorized();
        
        }

        private string GenerateJwtToken(string username)
        {
    var user = _context.Users.FirstOrDefault(u => u.Username == username);

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim("UserId", user.Id.ToString()), // Include UserId in the token
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(30), 
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
