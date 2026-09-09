using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarVehiculo;

public class AgregarVehiculoCommandHandler
    : IRequestHandler<AgregarVehiculoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var sucursalId = _currentUser.SucursalId;

        if (sucursalId == Guid.Empty)
        {
            throw new UnauthorizedAccessException(
                "El usuario autenticado no tiene una sucursal asignada."
            );
        }

        var vehiculo = Vehiculo.Crear(
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
            sucursalId,
            empresaId);

        await _context.Vehiculos.AddAsync(
            vehiculo,
            cancellationToken
        );

        await _context.SaveChangesAsync(cancellationToken);

        return vehiculo.Id;
    }
}
