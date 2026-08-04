using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.ActualizarMarcaVehiculo;

public class ActualizarMarcaVehiculoCommandHandler
    : IRequestHandler<ActualizarMarcaVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarMarcaVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarMarcaVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerMarcaId = await _context.MarcasVehiculos.FirstOrDefaultAsync(
            m => m.Id == request.MarcaVehiculoId && m.EmpresaId == empresaId && !m.Eliminado,
            cancellationToken
        );

        if (obtenerMarcaId == null)
            throw new Exception("Marca de vehículo no encontrada.");

        obtenerMarcaId.ActualizarMarcaVehiculo(request.Nombre);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
