
using Concesionaria.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TuProyecto.Application.Auth.Commands.CambiarContraseña
{
    public class CambiarContraseñaHandler : IRequestHandler<CambiarContraseñaCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CambiarContraseñaHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(CambiarContraseñaCommand request, CancellationToken ct)
        {
            var contactoLimpio = request.Contacto.Replace(" ", "").Replace("+", "").Replace("-", "");
            var usuario = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.Email == request.Contacto ||
                (u.Telefono != null && u.Telefono.Replace(" ", "").Replace("+", "").Replace("-", "") == contactoLimpio),
                ct);

            if (usuario == null) return false;

            var removeResult = await _userManager.RemovePasswordAsync(usuario);

            var addResult = await _userManager.AddPasswordAsync(usuario, request.NuevaPassword);

            return addResult.Succeeded;
        }
    }
}