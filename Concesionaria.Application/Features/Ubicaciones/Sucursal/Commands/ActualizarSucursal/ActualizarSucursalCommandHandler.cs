using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Ubicaciones.Commands.ActualizarSucursal;

public class ActualizarSucursalCommandHandler : IRequestHandler<ActualizarSucursalCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarSucursalCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarSucursalCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerSucursalId = await _context.Sucursales.FirstOrDefaultAsync(
            s => s.Id == request.SucursalId && s.EmpresaId == empresaId && !s.Eliminado,
            cancellationToken
        );

        if (obtenerSucursalId == null)
            throw new Exception("Sucursal no encontrada.");

        var Actualizar = false;

        if (!string.IsNullOrWhiteSpace(request.Nombre))
        {
            var nombreNormalizado = request.Nombre.Trim().ToUpper();
            if (obtenerSucursalId.Nombre != nombreNormalizado)
            {
                obtenerSucursalId.ActualizarNombreSucursal(request.Nombre);
                Actualizar = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Direccion))
        {
            var direccionNormalizada = request.Direccion.Trim().ToUpper();
            if (obtenerSucursalId.Direccion != direccionNormalizada)
            {
                obtenerSucursalId.ActualizarDireccionSucursal(request.Direccion);
                Actualizar = true;
            }
        }

        if (
            request.LocalidadId != Guid.Empty
            && obtenerSucursalId.LocalidadId != request.LocalidadId
        )
        {
            if (obtenerSucursalId.LocalidadId != request.LocalidadId)
            {
                obtenerSucursalId.ActualizarLocalidadSucursal(request.LocalidadId);
                Actualizar = true;
            }
        }

        if (Actualizar)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        if (!Actualizar)
        {
            throw new Exception("No se realizaron cambios en la sucursal.");
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
