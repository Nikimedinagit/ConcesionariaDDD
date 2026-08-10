using MediatR;

namespace Application.Features.Vehiculos.Commands.ActualizarTipoVehiculo;

public record ActualizarTipoVehiculoCommand : IRequest<Unit>{
    public Guid TipoVehiculoId { get; init; }
    public string Nombre { get; init; } = string.Empty;
}
