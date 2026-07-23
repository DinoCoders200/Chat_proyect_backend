using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using custom_chat_backend.Core.Domain.Entities.User;
using custom_chat_backend.Core.Interfaces.Auth;
using Microsoft.IdentityModel.Tokens;

namespace custom_chat_backend.Infrastructure.Services.Common;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateAccessToken(UserEntity user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                ClaimTypes.Name,
                user.Username)
        };


        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                configuration["Jwt:Key"]!));


        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);


        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }


    public string GenerateRefreshToken()
    {
        var bytes = Guid.NewGuid().ToByteArray();

        return Convert.ToBase64String(bytes);
    }
}