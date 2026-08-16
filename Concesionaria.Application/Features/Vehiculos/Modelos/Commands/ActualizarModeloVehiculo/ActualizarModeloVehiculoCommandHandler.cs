using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.ActualizarModeloVehiculo;

public class ActualizarModeloVehiculoCommandHandler
    : IRequestHandler<ActualizarModeloVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarModeloVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarModeloVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerModeloId = await _context.ModelosVehiculos.FirstOrDefaultAsync(
            m => m.Id == request.ModeloVehiculoId && m.EmpresaId == empresaId && !m.Eliminado,
            cancellationToken
        );

        if (obtenerModeloId == null)
            throw new Exception("Modelo de vehículo no encontrado.");

        obtenerModeloId.ActualizarModeloVehiculo(request.Nombre, request.MarcaVehiculoId, request.TipoVehiculoId);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
