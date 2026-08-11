using MediatR;

namespace Application.Features.Vehiculos.Commands.DesactivarTipoVehiculo;

public record DesactivarTipoVehiculoCommand : IRequest<Unit>{
    public Guid TipoVehiculoId { get; init; }
    public bool Eliminado { get; init; }
}
