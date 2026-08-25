#nullable enable
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Controllers;
using Concesionaria.Application.Common.Authorization;
using Concesionaria.Api.Authorization;
using Concesionaria.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace TuProyecto.Api.Middleware;

public sealed class PermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(
        HttpContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var descriptor = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
        var declaredPermission = context.GetEndpoint()?.Metadata
            .GetOrderedMetadata<RequirePermissionAttribute>()
            .FirstOrDefault()?.Permission;
        var requiredPermission = declaredPermission ?? ResolvePermission(context, descriptor);

        if (requiredPermission is null || IsAdministrator(context.User))
        {
            await _next(context);
            return;
        }

        var currentPermissions = await GetCurrentPermissions(context.User, userManager, roleManager);

        if (currentPermissions.Contains("ADMINISTRATOR"))
        {
            await _next(context);
            return;
        }

        var allowed = currentPermissions.Contains(requiredPermission);

        if (!allowed)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "No tenés permisos para realizar esta acción.",
                permission = requiredPermission,
            });
            return;
        }

        await _next(context);
    }

    private static async Task<HashSet<string>> GetCurrentPermissions(
        ClaimsPrincipal principal,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var userId = principal.FindFirst("nameid")?.Value;
        var user = userId is null ? null : await userManager.FindByIdAsync(userId);
        if (user is null) return permissions;

        var roles = await userManager.GetRolesAsync(user);
        foreach (var roleName in roles)
        {
            if (string.Equals(roleName, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase))
            {
                permissions.Add("ADMINISTRATOR");
                return permissions;
            }

            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null) continue;
            var claims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in claims.Where(c => c.Type == PermissionCatalog.Action))
                permissions.Add(claim.Value);
        }

        var userClaims = await userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims.Where(c => c.Type == PermissionCatalog.Action))
            permissions.Add(claim.Value);

        return permissions;
    }

    private static string? ResolvePermission(HttpContext context, ControllerActionDescriptor? descriptor)
    {
        if (descriptor is null) return null;

        var prefix = descriptor.ControllerName switch
        {
            "Usuarios" => "USUARIOS",
            "Clientes" => "CLIENTES",
            "Proveedores" => "PROVEEDORES",
            "Cuentas" => "CUENTAS",
            "CategoriasGastos" => "CATEGORIAS_GASTOS",
            "Sucursales" => "SUCURSALES",
            "Localidades" => "LOCALIDADES",
            "Provincias" => "PROVINCIAS",
            "MarcasVehiculos" => "MARCAS",
            "TiposVehiculos" => "TIPOS_VEHICULO",
            "ModelosVehiculos" => "MODELOS",
            _ => null,
        };

        if (prefix is null) return null;

        var route = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
        var action = context.Request.Method switch
        {
            "GET" => "VER",
            "POST" => "CREAR",
            "DELETE" => "DESACTIVAR",
            "PUT" when route.Contains("/activar/") => "ACTIVAR",
            "PUT" when route.Contains("/desactivar/") => "DESACTIVAR",
            "PUT" => "EDITAR",
            _ => null,
        };

        return action is null ? null : $"{prefix}_{action}";
    }

    private static bool IsAdministrator(ClaimsPrincipal user) =>
        user.Claims.Any(claim =>
            (claim.Type == "role" || claim.Type == ClaimTypes.Role) &&
            string.Equals(claim.Value, "ADMINISTRADOR", StringComparison.OrdinalIgnoreCase));
}
