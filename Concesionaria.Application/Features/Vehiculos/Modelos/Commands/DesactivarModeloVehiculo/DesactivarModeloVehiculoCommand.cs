using MediatR;

namespace Application.Features.Vehiculos.Commands.DesactivarModeloVehiculo;

public record DesactivarModeloVehiculoCommand : IRequest<Unit>{
    public Guid ModeloVehiculoId { get; init; }
    public bool Eliminado { get; init; }
}
