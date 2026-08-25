using Microsoft.AspNetCore.Identity;
using Concesionaria.Application.Common.Authorization;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "ADMINISTRADOR", "VENDEDOR", "CONTADOR", "GESTOR", "TALLER" };

        foreach (var rol in roles)
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }

            var roleEntity = await roleManager.FindByNameAsync(rol);
            if (roleEntity is null) continue;

            var existingClaims = await roleManager.GetClaimsAsync(roleEntity);
            var permissions = rol == "ADMINISTRADOR"
                ? PermissionCatalog.All.Keys
                : DefaultPermissions(rol);

            foreach (var permission in permissions)
            {
                if (existingClaims.Any(c => c.Type == PermissionCatalog.Action && c.Value == permission))
                    continue;

                await roleManager.AddClaimAsync(
                    roleEntity,
                    new System.Security.Claims.Claim(PermissionCatalog.Action, permission));
            }
        }
    }

    private static IEnumerable<string> DefaultPermissions(string role) => role switch
    {
        "VENDEDOR" => new[] { "PERFIL_VER", "CLIENTES_VER", "CLIENTES_CREAR", "CLIENTES_EDITAR", "PROVEEDORES_VER" },
        "CONTADOR" => new[] { "PERFIL_VER", "CONTABILIDAD_VER", "TESORERIA_VER" },
        "GESTOR" => new[] { "PERFIL_VER", "CLIENTES_VER", "PROVEEDORES_VER", "SUCURSALES_VER" },
        "TALLER" => new[] { "PERFIL_VER", "VEHICULOS_VER" },
        _ => Array.Empty<string>(),
    };
}
