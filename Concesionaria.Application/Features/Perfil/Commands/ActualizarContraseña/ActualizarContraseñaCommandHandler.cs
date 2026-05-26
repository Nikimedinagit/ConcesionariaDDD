using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Concesionaria.Domain.Identity;

public class ActualizarContraseñaCommandHandler : IRequestHandler<ActualizarContraseñaCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActualizarContraseñaCommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(ActualizarContraseñaCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var usuario = await _userManager.FindByIdAsync(userId);

        if (usuario == null) return false;

        var esValida = await _userManager.CheckPasswordAsync(usuario, request.PasswordActual);

        if (!esValida)
        {
            throw new Exception("Contraseña incorrecta.");
        }

        var resultado = await _userManager.ChangePasswordAsync(usuario, request.PasswordActual, request.PasswordNueva);

        return resultado.Succeeded;
    }
}