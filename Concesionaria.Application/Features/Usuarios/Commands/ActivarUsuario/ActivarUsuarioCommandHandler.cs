using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Usuarios.Commands.ActivarUsuario;

public class ActivarUsuarioCommandHandler : IRequestHandler<ActivarUsuarioCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;

    public ActivarUsuarioCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        UserManager<ApplicationUser> userManager
    )
    {
        _context = context;
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(
        ActivarUsuarioCommand request,
        CancellationToken cancellationToken
    )
    {
        var usuario = await _context.Usuarios
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(
                u => u.Id == request.UsuarioId
                    && u.EmpresaId == _currentUser.EmpresaId
                    && u.Eliminado,
                cancellationToken
            );

        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        var identityUser = await _userManager.FindByEmailAsync(usuario.Email);

        if (identityUser == null)
            throw new Exception("Usuario de acceso no encontrado.");

        identityUser.InvalidarSesion();
        identityUser.LockoutEnd = null;
        var identityResult = await _userManager.UpdateAsync(identityUser);

        if (!identityResult.Succeeded)
        {
            var mensaje = string.Join(" ", identityResult.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        usuario.Activar();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
