using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.ActivarProveedor;

public class ActivarProveedorCommandHandler
    : IRequestHandler<ActivarProveedorCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarProveedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarProveedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        
        var obtenerProveedorId = await _context.Proveedores.IgnoreQueryFilters().FirstOrDefaultAsync(
            m => m.Id == request.ProveedorId && m.EmpresaId == empresaId && m.Eliminado,
            cancellationToken
        );

        if (obtenerProveedorId == null)
            throw new Exception("Proveedor no encontrado.");

        obtenerProveedorId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
