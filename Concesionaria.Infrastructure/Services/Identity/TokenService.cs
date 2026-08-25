using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Concesionaria.Domain.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Concesionaria.Application.Common.Authorization;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(
        IConfiguration config,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["TokenKey"]!)
        );
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<string> CreateToken(ApplicationUser user)
    {
        var roleName = string.Empty;

        if (!string.IsNullOrWhiteSpace(user.RolId))
        {
            var role = await _roleManager.FindByIdAsync(user.RolId);
            roleName = role?.Name ?? string.Empty;
        }

        if (string.IsNullOrWhiteSpace(roleName))
        {
            var roles = await _userManager.GetRolesAsync(user);
            roleName = roles.FirstOrDefault() ?? string.Empty;
        }

        var claims = new List<Claim>
        {
            new Claim("nameid", user.Id),
            new Claim("email", user.Email!),
            new Claim("nombre", user.NombreCompleto),
            new Claim("avatarUrl", user.AvatarUrl ?? ""),
            new Claim("EmpresaId", user.EmpresaId.ToString()),
            new Claim("role", roleName),
            new Claim(ClaimTypes.Role, roleName)

        };

        var roleClaims = await _roleManager.GetClaimsAsync(
            await _roleManager.FindByNameAsync(roleName) ?? new IdentityRole());
        var userClaims = await _userManager.GetClaimsAsync(user);

        claims.AddRange(roleClaims.Where(c => c.Type == PermissionCatalog.Action));
        claims.AddRange(userClaims.Where(c => c.Type == PermissionCatalog.Action));

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
