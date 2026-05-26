using Concesionaria.Application.Auth.Commands.SolicitarCodigo;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class SolicitarCodigoCommandHandler : IRequestHandler<SolicitarCodigoCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;

    public SolicitarCodigoCommandHandler(
        UserManager<ApplicationUser> userManager,
        INotificationService notificationService)
    {
        _userManager = userManager;
        _notificationService = notificationService;
    }

    public async Task<bool> Handle(SolicitarCodigoCommand request, CancellationToken cancellationToken)
    {
        var contactoLimpio = request.Contacto.Replace(" ", "").Replace("+", "").Replace("-", "");

        var usuario = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.Contacto ||
                (u.Telefono != null && u.Telefono.Replace(" ", "").Replace("+", "").Replace("-", "") == contactoLimpio),
                cancellationToken
            );

        if (usuario == null)
        
        {
            throw new Exception("El email o teléfono no está registrado.");
        }
        string codigo = new Random().Next(100000, 999999).ToString();
        usuario.EstablecerCodigoRecuperacion(codigo);

        await _userManager.UpdateAsync(usuario);

        await _notificationService.SendNotificationAsync(
            request.Contacto,
            $"Tu código de verificación es: {codigo}. Este código expira en 5 minutos."
        );

        return true;
    }
}