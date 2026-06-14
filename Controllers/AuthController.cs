using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotNetMVCWebApp1.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace dotNetMVCWebApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        string role;

        // simple demo users
        if (request.Username == "admin" && request.Password == "123")
            role = "Admin";
        else if (request.Username == "roey" && request.Password == "123")
            role = "Admin";
        else if (request.Username == "user" && request.Password == "123")
            role = "User";
        else
            return Unauthorized();
        // Duplicate use of secret key; in a real app, use configuration or a secure vault
        var key = Encoding.UTF8.GetBytes("ThisIsMyVeryLongSecretKeyForJwtDemo123456");

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new { token = jwt, role });
    }
}