using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Concesionaria.Domain.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Concesionaria.Application.Common.Authorization;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public TokenService(
        IConfiguration config,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        _key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(config["TokenKey"]!)
        );
        _roleManager = roleManager;
        _userManager = userManager;
        _dbContext = dbContext;
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
            new Claim("empresaId", user.EmpresaId.ToString()),
            new Claim("sessionVersion", user.SessionVersion.ToString()),
            new Claim("role", roleName),
            new Claim(ClaimTypes.Role, roleName)
        };

        if (user.SucursalId.HasValue)
        {
            var sucursalNombre = await _dbContext.Sucursales
                .IgnoreQueryFilters()
                .Where(sucursal =>
                    sucursal.Id == user.SucursalId.Value &&
                    sucursal.EmpresaId == user.EmpresaId &&
                    !sucursal.Eliminado)
                .Select(sucursal => sucursal.Nombre)
                .FirstOrDefaultAsync();

            claims.Add(
                new Claim("sucursalId", user.SucursalId.Value.ToString())
            );

            claims.Add(
                new Claim("sucursalNombre", sucursalNombre ?? string.Empty)
            );
        }

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
