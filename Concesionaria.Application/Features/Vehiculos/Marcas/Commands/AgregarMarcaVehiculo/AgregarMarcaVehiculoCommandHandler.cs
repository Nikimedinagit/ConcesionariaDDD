using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarMarcaVehiculo;

public class AgregarMarcaVehiculoCommandHandler
    : IRequestHandler<AgregarMarcaVehiculoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarMarcaVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarMarcaVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var marcaVehiculo = MarcaVehiculo.Crear(request.Nombre, empresaId);

        await _context.MarcasVehiculos.AddAsync(marcaVehiculo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return marcaVehiculo.Id;
    }
}
