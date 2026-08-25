using System.Security.Claims;
using Concesionaria.Application.Common.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Concesionaria.Domain.Identity;

namespace Concesionaria.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PermisosController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public PermisosController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet("mis-permisos")]
    public async Task<IActionResult> ObtenerMisPermisos()
    {
        var userId = User.FindFirstValue("nameid");
        var user = userId is null ? null : await _userManager.FindByIdAsync(userId);
        if (user is null) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role is null) continue;
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims.Where(c => c.Type == PermissionCatalog.Action))
                permissions.Add(claim.Value);
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims.Where(c => c.Type == PermissionCatalog.Action))
            permissions.Add(claim.Value);

        if (roles.Any(role => string.Equals(role, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)))
            permissions.UnionWith(PermissionCatalog.All.Keys);

        return Ok(permissions.OrderBy(permission => permission));
    }

    [HttpGet("catalogo")]
    public async Task<IActionResult> ObtenerCatalogo()
    {
        if (!User.IsInRole("ADMINISTRADOR") &&
            !User.Claims.Any(claim =>
                claim.Type == "role" &&
                string.Equals(claim.Value, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)))
        {
            return Forbid();
        }

        var roles = new List<object>();

        foreach (var role in _roleManager.Roles.OrderBy(r => r.Name))
        {
            var claims = await _roleManager.GetClaimsAsync(role);
            roles.Add(new
            {
                id = role.Id,
                nombre = role.Name,
                permisos = claims
                    .Where(c => c.Type == PermissionCatalog.Action)
                    .Select(c => c.Value)
                    .OrderBy(value => value)
                    .ToList(),
            });
        }

        return Ok(new
        {
            permisos = PermissionCatalog.All.Select(permission => new
            {
                codigo = permission.Key,
                nombre = permission.Value,
                vista = permission.Key.Split('_')[0],
                accion = permission.Key[(permission.Key.IndexOf('_') + 1)..],
            }),
            roles,
        });
    }

    [HttpPut("roles/{roleId}")]
    public async Task<IActionResult> ActualizarRol(string roleId, [FromBody] ActualizarPermisosRequest request)
    {
        if (!User.IsInRole("ADMINISTRADOR") &&
            !User.Claims.Any(claim =>
                claim.Type == "role" &&
                string.Equals(claim.Value, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase)))
        {
            return Forbid();
        }

        var role = await _roleManager.FindByIdAsync(roleId);
        if (role is null) return NotFound(new { message = "Rol no encontrado." });

        var permitidos = PermissionCatalog.All.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var nuevos = request.Permisos
            .Where(permission => permitidos.Contains(permission))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var actuales = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in actuales.Where(c => c.Type == PermissionCatalog.Action).ToList())
            await _roleManager.RemoveClaimAsync(role, claim);

        foreach (var permission in nuevos)
            await _roleManager.AddClaimAsync(role, new Claim(PermissionCatalog.Action, permission));

        return Ok(new { message = "Permisos actualizados correctamente." });
    }
}

public class ActualizarPermisosRequest
{
    public List<string> Permisos { get; set; } = [];
}
