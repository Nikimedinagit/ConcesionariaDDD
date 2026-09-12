using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Cuentas.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.EliminarVehiculo;

public class EliminarVehiculoCommandHandler : IRequestHandler<EliminarVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public EliminarVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        EliminarVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var sucursalId = _currentUser.SucursalId;

        if (sucursalId == Guid.Empty)
            throw new UnauthorizedAccessException("El usuario no tiene una sucursal asignada.");

        var vehiculo = await _context.Vehiculos.FirstOrDefaultAsync(
            vehiculo =>
                vehiculo.Id == request.VehiculoId
                && vehiculo.EmpresaId == empresaId
                && vehiculo.SucursalId == sucursalId
                && vehiculo.Estado == EstadoVehiculo.DISPONIBLE,
            cancellationToken
        );

        if (vehiculo == null)
            throw new Exception("Solo se pueden eliminar vehículos disponibles.");

        _context.Vehiculos.Remove(vehiculo);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
