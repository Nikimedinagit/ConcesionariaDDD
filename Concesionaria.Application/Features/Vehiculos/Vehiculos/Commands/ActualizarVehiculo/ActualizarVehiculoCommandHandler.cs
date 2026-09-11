using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Cuentas.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.ActualizarVehiculo;

public class ActualizarVehiculoCommandHandler
    : IRequestHandler<ActualizarVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var sucursalId = _currentUser.SucursalId;

        if (sucursalId == Guid.Empty)
            throw new UnauthorizedAccessException("El usuario no tiene una sucursal asignada.");

        var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(
            vehiculo =>
                vehiculo.Id == request.VehiculoId &&
                vehiculo.EmpresaId == empresaId &&
                vehiculo.SucursalId == sucursalId &&
                vehiculo.Estado != EstadoVehiculo.VENDIDO,
            cancellationToken
        );

        if (vehiculo == null)
            throw new Exception("Vehículo no encontrado.");

        vehiculo.ActualizarVehiculo(
            request.Version,
            request.Patente,
            request.Color,
            request.Anio,
            request.Kilometraje,
            request.Condicion,
            request.Estado,
            request.PrecioCompra,
            request.PrecioVenta,
            request.ModeloId,
            sucursalId);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
