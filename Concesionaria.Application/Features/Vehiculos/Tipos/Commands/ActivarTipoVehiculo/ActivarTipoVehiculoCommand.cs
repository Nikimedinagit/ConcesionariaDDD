using MediatR;
namespace Application.Features.Vehiculos.Commands.ActivarTipoVehiculo;

public record ActivarTipoVehiculoCommand : IRequest<Unit>{
    public Guid TipoVehiculoId { get; init; }
    public bool Eliminado { get; init; }
}
