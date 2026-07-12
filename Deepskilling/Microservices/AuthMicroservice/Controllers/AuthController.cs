using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AuthMicroservice.Models;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (IsValidUser(model))
            {
                // Generate token with claims including name and role (Questions 1 and 3)
                var token = GenerateJwtToken(model.Username, model.Role);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid username or password" });
        }

        private bool IsValidUser(LoginModel model)
        {
            // Simple mock validation: accept any user if password matches 'password'
            return !string.IsNullOrWhiteSpace(model.Username) && model.Password == "password";
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role), // Question 3: Role-based Authorization claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtKey = _configuration["Jwt:Key"] ?? "ThisIsASecretKeyForJwtTokenSecretKeyString!!12345";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration["Jwt:Issuer"] ?? "MyAuthServer";
            var audience = _configuration["Jwt:Audience"] ?? "MyApiUsers";
            var durationMinutes = double.Parse(_configuration["Jwt:DurationInMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(durationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
