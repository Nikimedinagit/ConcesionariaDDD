using MediatR;
namespace Application.Features.Vehiculos.Commands.ActivarModeloVehiculo;

public record ActivarModeloVehiculoCommand : IRequest<Unit>{
    public Guid ModeloVehiculoId { get; init; }
    public bool Eliminado { get; init; }
}
