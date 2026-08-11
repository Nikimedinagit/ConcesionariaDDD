using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Vehiculos.Commands.DesactivarTipoVehiculo;

public class DesactivarTipoVehiculoCommandHandler
    : IRequestHandler<DesactivarTipoVehiculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DesactivarTipoVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        DesactivarTipoVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var res = request;
        
        var obtenerTipoId = await _context.TiposVehiculos.IgnoreQueryFilters().FirstOrDefaultAsync(
            tv => tv.Id == request.TipoVehiculoId && tv.EmpresaId == empresaId && !tv.Eliminado,
            cancellationToken
        );

        if (obtenerTipoId == null)
            throw new Exception("Tipo de vehículo no encontrado.");

        obtenerTipoId.Desactivar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}