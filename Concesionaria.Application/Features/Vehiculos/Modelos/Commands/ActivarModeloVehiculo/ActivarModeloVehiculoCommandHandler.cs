using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.ActivarModeloVehiculo;

public class ActivarModeloVehiculoCommandHandler
    : IRequestHandler<ActivarModeloVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarModeloVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarModeloVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var res = request;
        
        var obtenerModeloId = await _context.ModelosVehiculos.IgnoreQueryFilters().FirstOrDefaultAsync(
            m => m.Id == request.ModeloVehiculoId && m.EmpresaId == empresaId && m.Eliminado,
            cancellationToken
        );

        if (obtenerModeloId == null)
            throw new Exception("Modelo de vehículo no encontrado.");

        obtenerModeloId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
