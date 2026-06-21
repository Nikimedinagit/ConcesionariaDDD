using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Usuarios.Commands.CambiarPasswordUsuario;

public class CambiarPasswordUsuarioCommandHandler
    : IRequestHandler<CambiarPasswordUsuarioCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;

    public CambiarPasswordUsuarioCommandHandler(
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
        CambiarPasswordUsuarioCommand request,
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

        var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
        var result = await _userManager.ResetPasswordAsync(
            identityUser,
            token,
            request.Password
        );

        if (!result.Succeeded)
        {
            var mensaje = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new Exception(mensaje);
        }

        usuario.ActualizarPassword(identityUser.PasswordHash);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
