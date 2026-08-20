using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class ActualizarProveedorCommandHandler
    : IRequestHandler<ActualizarProveedorCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarProveedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarProveedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerProveedorId = await _context.Proveedores.FirstOrDefaultAsync(
            m => m.Id == request.ProveedorId && m.EmpresaId == empresaId && !m.Eliminado,
            cancellationToken
        );

        if (obtenerProveedorId == null)
            throw new Exception("Proveedor no encontrado.");

        obtenerProveedorId.ActualizarProveedor(request.Nombre, request.Cuil, request.Telefono, request.Email, request.Domicilio, request.Servicio, request.Observacion, request.LocalidadId);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
