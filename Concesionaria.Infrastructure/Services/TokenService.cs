using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Concesionaria.Domain.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Concesionaria.Application.Common.Interfaces;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["TokenKey"]!)
        );
    }

    public string CreateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("nameid", user.Id),
            new Claim("email", user.Email!),
            new Claim("nombre", user.NombreCompleto),
            new Claim("avatarUrl", user.AvatarUrl ?? "")
        };

        var creds = new SigningCredentials(
            _key,
            SecurityAlgorithms.HmacSha512Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.AddDays(7),

            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}