using MediatR;

namespace Application.Features.Vehiculos.Commands.DesactivarMarcaVehiculo;

public record DesactivarMarcaVehiculoCommand : IRequest<Unit>{
    public Guid MarcaVehiculoId { get; init; }
    public bool Eliminado { get; init; }
}
