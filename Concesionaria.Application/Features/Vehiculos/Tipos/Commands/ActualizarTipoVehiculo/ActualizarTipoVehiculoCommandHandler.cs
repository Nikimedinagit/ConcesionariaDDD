using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.ActualizarTipoVehiculo;

public class ActualizarTipoVehiculoCommandHandler
    : IRequestHandler<ActualizarTipoVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarTipoVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarTipoVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerTipoId = await _context.TiposVehiculos.FirstOrDefaultAsync(
            t => t.Id == request.TipoVehiculoId && t.EmpresaId == empresaId && !t.Eliminado,
            cancellationToken
        );

        if (obtenerTipoId == null)
            throw new Exception("Tipo de vehículo no encontrado.");

        obtenerTipoId.ActualizarTipoVehiculo(request.Nombre);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
