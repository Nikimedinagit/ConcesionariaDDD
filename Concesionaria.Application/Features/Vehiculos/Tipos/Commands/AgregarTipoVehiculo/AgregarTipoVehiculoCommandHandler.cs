using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarTipoVehiculo;

public class AgregarTipoVehiculoCommandHandler
    : IRequestHandler<AgregarTipoVehiculoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarTipoVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarTipoVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var tipoVehiculo = TipoVehiculo.Crear(request.Nombre, empresaId);

        await _context.TiposVehiculos.AddAsync(tipoVehiculo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return tipoVehiculo.Id;
    }
}
