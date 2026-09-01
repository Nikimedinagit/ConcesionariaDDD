using Concesionaria.Application.Common.Interfaces;
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

        var obtenerVehiculoId = await _context.Vehiculos.FirstOrDefaultAsync(
            m => m.Id == request.VehiculoId && m.EmpresaId == empresaId && m.Estado != EstadoVehiculo.Vendido,
            cancellationToken
        );

        if (obtenerVehiculoId == null)
            throw new Exception("Vehículo no encontrada.");

        obtenerVehiculoId.ActualizarVehiculo(request.Version, request.Patente, request.Color, request.Anio, request.Kilometraje, request.Condicion, request.Estado, request.PrecioCompra, request.PrecioVenta, request.ModeloId, request.SucursalId);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
