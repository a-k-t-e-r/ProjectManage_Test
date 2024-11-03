using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjManagAppForOpteam.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjManagAppForOpteam.Auth;

public class AuthService(IConfiguration config)
{
    private readonly IConfiguration _config = config;
    DateTime TokenLifeTime = DateTime.UtcNow.AddHours(1);

    public string GenerateJwtToken([FromBody] User user)
    {
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = TokenLifeTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(securityTokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}