using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarModeloVehiculo;

public class AgregarModeloVehiculoCommandHandler
    : IRequestHandler<AgregarModeloVehiculoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarModeloVehiculoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarModeloVehiculoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var modeloVehiculo = ModeloVehiculo.Crear(request.Nombre, empresaId, request.MarcaVehiculoId, request.TipoVehiculoId);

        await _context.ModelosVehiculos.AddAsync(modeloVehiculo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return modeloVehiculo.Id;
    }
}
