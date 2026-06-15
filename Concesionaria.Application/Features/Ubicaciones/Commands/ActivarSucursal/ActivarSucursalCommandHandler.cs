using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Ubicaciones.Commands.ActivarSucursal;

public class ActivarSucursalCommandHandler
    : IRequestHandler<ActivarSucursalCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarSucursalCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarSucursalCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        
        var obtenerSucursalId = await _context.Sucursales.IgnoreQueryFilters().FirstOrDefaultAsync(
            s => s.Id == request.SucursalId && s.EmpresaId == empresaId && s.Eliminado,
            cancellationToken
        );

        if (obtenerSucursalId == null)
            throw new Exception("Sucursal no encontrada.");

        obtenerSucursalId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
