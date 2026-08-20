using Concesionaria.Application.Common.Interfaces;
using MediatR;

public class AgregarProveedorCommandHandler
    : IRequestHandler<AgregarProveedorCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarProveedorCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarProveedorCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var proveedor = Proveedor.Crear(request.Nombre, request.Cuil, request.Telefono, request.Email, request.Domicilio, request.Servicio, request.Observacion, request.LocalidadId, empresaId);

        await _context.Proveedores.AddAsync(proveedor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return proveedor.Id;
    }
}
