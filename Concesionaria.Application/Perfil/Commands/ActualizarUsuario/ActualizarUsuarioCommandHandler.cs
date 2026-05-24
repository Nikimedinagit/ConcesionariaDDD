using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Concesionaria.Application.Perfil.Commands.ActualizarUsuario;

public class ActualizarUsuarioCommandHandler
    : IRequestHandler<ActualizarUsuarioCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ActualizarUsuarioCommandHandler(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(
        ActualizarUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        var usuario = await _userManager
            .FindByIdAsync(request.UsuarioId);

        if (usuario == null)
            throw new Exception("Usuario no encontrado");

        usuario.ActualizarNombre(
            request.NombreCompleto);

        usuario.ActualizarTelefono(
            request.Telefono ?? "");

        usuario.ActualizarAvatar(
            request.AvatarUrl ?? "");

        var result = await _userManager
            .UpdateAsync(usuario);

        return result.Succeeded;
    }
}