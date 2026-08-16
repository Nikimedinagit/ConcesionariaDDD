using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.DesactivarModeloVehiculo;

public class DesactivarModeloVehiculoCommandHandler
    : IRequestHandler<DesactivarModeloVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DesactivarModeloVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        DesactivarModeloVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var res = request;
        
        var obtenerModeloId = await _context.ModelosVehiculos.IgnoreQueryFilters().FirstOrDefaultAsync(
            mv => mv.Id == request.ModeloVehiculoId && mv.EmpresaId == empresaId && !mv.Eliminado,
            cancellationToken
        );

        if (obtenerModeloId == null)
            throw new Exception("Modelo de vehículo no encontrado.");

        obtenerModeloId.Desactivar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}