using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.Application.Perfil.Queries.GetPerfil;

public class GetPerfilQueryHandler
    : IRequestHandler<GetPerfilQuery, PerfilDto>
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    public GetPerfilQueryHandler(
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _context = context;
        _roleManager = roleManager;
    }

    public async Task<PerfilDto> Handle(
        GetPerfilQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        Console.WriteLine($"USER ID: {userId}");


        if (userId == null)
            throw new Exception("Usuario no autenticado");

        var usuario = await _userManager.Users
            .Include(x => x.Empresa)
            .ThenInclude(x => x.Localidad)
            .FirstOrDefaultAsync(
                x => x.Id == userId,
                cancellationToken);


        Console.WriteLine($"USUARIO NULL: {usuario == null}");

        Console.WriteLine($"EMPRESA NULL: {usuario?.Empresa == null}");

        Console.WriteLine($"LOCALIDAD NULL: {usuario?.Empresa?.Localidad == null}");

        Console.WriteLine($"TELEFONO: {usuario?.Telefono}");

        Console.WriteLine($"AVATAR: {usuario?.AvatarUrl}");

        if (usuario == null)
            throw new Exception("Usuario no encontrado");

        var usuarioSistema = await _context.Usuarios
            .Include(x => x.Sucursal)
            .FirstOrDefaultAsync(x => x.Email == usuario.Email, cancellationToken);

        var rol = !string.IsNullOrWhiteSpace(usuario.RolId)
            ? await _roleManager.FindByIdAsync(usuario.RolId)
            : null;

        var rolNombre = rol?.Name;
        if (string.IsNullOrWhiteSpace(rolNombre))
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            rolNombre = roles.FirstOrDefault() ?? string.Empty;
        }

        var sucursalNombre = string.Equals(
            rolNombre,
            "ADMINISTRADOR",
            StringComparison.OrdinalIgnoreCase)
            ? "TODAS LAS SUCURSALES"
            : usuarioSistema?.Sucursal?.Nombre ?? "Sin asignar";



        return new PerfilDto
        {
            EmpresaId = usuario.Empresa.Id,

            RazonSocial = usuario.Empresa.RazonSocial,

            Cuit = usuario.Empresa.Cuit,

            NombreFantasia = usuario.Empresa.NombreFantasia,

            Moneda = usuario.Empresa.MonedaPrincipal.ToString(),

            LocalidadId = usuario.Empresa.LocalidadId,

            Activa = usuario.Empresa.Activa,

            UsuarioId = usuario.Id,

            NombreCompleto = usuario.NombreCompleto,

            Email = usuario.Email ?? "",

            Telefono = usuario.Telefono,

            AvatarUrl = usuario.AvatarUrl
                ,RolNombre = rolNombre
                ,SucursalNombre = sucursalNombre
        };
    }
}
