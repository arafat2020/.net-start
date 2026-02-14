using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentApi.Data;
using PaymentApi.Models;
using PaymentApi.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dto.auth;

namespace PaymentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AuthController(IConfiguration configuration, ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _context = context;
            _environment = environment;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromForm] SignupModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Username or Email already exists."));
            }

            string profilePicPath = string.Empty;
            if (model.ProfilePicture != null)
            {
                profilePicPath = await FileHelper.SaveProfilePicture(model.ProfilePicture, _environment.WebRootPath);
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                ProfilePicturePath = profilePicPath
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse<string>.SuccessResponse("User created successfully."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            

            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user.Username, user.Id, user.Email);
                return Ok(ApiResponse<object>.SuccessResponse(new { token, profilePicture = user.ProfilePicturePath }, "Login successful"));
            }

            return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid username or password"));
        }

        private string GenerateJwtToken(string username, int userId, string email)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, username),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(3),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

   
}
