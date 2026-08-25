using Concesionaria.Application.Auth.Commands.ValidarCodigo;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class ValidarCodigoCommandHandler : IRequestHandler<ValidarCodigoCommand, string>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public ValidarCodigoCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(ValidarCodigoCommand request, CancellationToken ct)
    {
        var contactoLimpio = request.Contacto.Replace(" ", "").Replace("+", "").Replace("-", "");

        var usuario = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.Contacto ||
                (u.Telefono != null && u.Telefono.Replace(" ", "").Replace("+", "").Replace("-", "") == contactoLimpio),
                ct);

        if (usuario == null)
        {
            throw new Exception("El email o teléfono no está registrado.");
        }

        if (usuario.CodigoRecuperacion != request.Codigo)
        {
            throw new Exception("Código incorrecto.");
        }

        if (usuario.ExpiracionCodigo < DateTime.UtcNow)
        {
            throw new Exception("El código ha expirado. Solicitá uno nuevo.");
        }

        usuario.LimpiarCodigoRecuperacion();
        await _userManager.UpdateAsync(usuario);

        return await _tokenService.CreateToken(usuario);
    }
}
