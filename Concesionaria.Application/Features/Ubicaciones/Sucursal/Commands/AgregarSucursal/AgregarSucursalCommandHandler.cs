using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Ubicaciones.Commands.AgregarSucursal;

public class AgregarSucursalCommandHandler : IRequestHandler<AgregarSucursalCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarSucursalCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarSucursalCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var sucursal = Sucursal.Crear(
            request.Nombre,
            request.Direccion,
            empresaId,
            request.LocalidadId
        );

        await _context.Sucursales.AddAsync(sucursal, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return sucursal.Id;
    }
}
