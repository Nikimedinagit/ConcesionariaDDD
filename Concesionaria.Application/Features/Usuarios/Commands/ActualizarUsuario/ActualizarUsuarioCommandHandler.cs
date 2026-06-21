using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Usuarios.Commands.ActualizarUsuario;

public class ActualizarUsuarioCommandHandler : IRequestHandler<ActualizarUsuarioCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ActualizarUsuarioCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _context = context;
        _currentUser = currentUser;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(
        ActualizarUsuarioCommand request,
        CancellationToken cancellationToken
    )
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(
            u => u.Id == request.UsuarioId
                && u.EmpresaId == _currentUser.EmpresaId
                && !u.Eliminado,
            cancellationToken
        );

        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        var identityUser = await _userManager.FindByEmailAsync(usuario.Email);

        if (identityUser == null)
            throw new Exception("Usuario de acceso no encontrado.");

        var role = await _roleManager.FindByIdAsync(request.RolId);

        if (role?.Name == null)
            throw new Exception("El rol no es válido.");

        identityUser.ActualizarNombre(request.NombreCompleto);
        identityUser.AsignarRol(request.RolId);

        var updateResult = await _userManager.UpdateAsync(identityUser);

        if (!updateResult.Succeeded)
        {
            var mensaje = string.Join(" ", updateResult.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        var currentRoles = await _userManager.GetRolesAsync(identityUser);

        if (currentRoles.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);

            if (!removeResult.Succeeded)
            {
                var mensaje = string.Join(" ", removeResult.Errors.Select(e => e.Description));
                throw new Exception(mensaje);
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(identityUser, role.Name);

        if (!addRoleResult.Succeeded)
        {
            var mensaje = string.Join(" ", addRoleResult.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        usuario.ActualizarDatos(
            request.RolId,
            request.SucursalId,
            request.NombreCompleto,
            usuario.Email
        );

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
