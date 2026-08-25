using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Concesionaria.Application.Perfil.Commands.ActualizarUsuario;

public class ActualizarUsuarioCommandHandler
    : IRequestHandler<ActualizarUsuarioCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly ICurrentUserService _currentUserService;

    private readonly ITokenService _tokenService;

    public ActualizarUsuarioCommandHandler(
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(
        ActualizarUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var usuario = await _userManager
            .FindByIdAsync(userId);

        if (usuario == null)
            throw new Exception("Usuario no encontrado");

        if (!string.IsNullOrEmpty(request.NombreCompleto))
        {
            usuario.ActualizarNombre(request.NombreCompleto);
        }

        if (request.Telefono != null)
        {
            usuario.ActualizarTelefono(request.Telefono);
        }

        if (request.AvatarUrl != null)
        {
            usuario.ActualizarAvatar(request.AvatarUrl);
        }

        usuario.ActualizarNombre(
            request.NombreCompleto);

        usuario.ActualizarTelefono(
            request.Telefono ?? "");

        usuario.ActualizarAvatar(
            request.AvatarUrl ?? "");

        var result = await _userManager
            .UpdateAsync(usuario);

        if (!result.Succeeded)
            throw new Exception("No se pudo actualizar");

        var token = await _tokenService.CreateToken(usuario);

        return token;
    }
}
