using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.ActivarMarcaVehiculo;

public class ActivarMarcaVehiculoCommandHandler
    : IRequestHandler<ActivarMarcaVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarMarcaVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarMarcaVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var res = request;
        
        var obtenerMarcaId = await _context.MarcasVehiculos.IgnoreQueryFilters().FirstOrDefaultAsync(
            m => m.Id == request.MarcaVehiculoId && m.EmpresaId == empresaId && m.Eliminado,
            cancellationToken
        );

        if (obtenerMarcaId == null)
            throw new Exception("Marca de vehículo no encontrada.");

        obtenerMarcaId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
