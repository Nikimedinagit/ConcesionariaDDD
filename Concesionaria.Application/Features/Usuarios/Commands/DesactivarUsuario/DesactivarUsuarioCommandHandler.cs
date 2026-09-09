using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Usuarios.Commands.DesactivarUsuario;

public class DesactivarUsuarioCommandHandler
    : IRequestHandler<DesactivarUsuarioCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;

    public DesactivarUsuarioCommandHandler(
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
        DesactivarUsuarioCommand request,
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

        identityUser.InvalidarSesion();
        identityUser.LockoutEnd = DateTimeOffset.MaxValue;
        
        var identityResult = await _userManager.UpdateAsync(identityUser);

        if (!identityResult.Succeeded)
        {
            var mensaje = string.Join(" ", identityResult.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        usuario.Desactivar();
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
