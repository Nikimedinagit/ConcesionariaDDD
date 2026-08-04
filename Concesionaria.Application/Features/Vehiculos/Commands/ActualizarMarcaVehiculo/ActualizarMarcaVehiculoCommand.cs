using MediatR;

namespace Application.Features.Vehiculos.Commands.ActualizarMarcaVehiculo;

public record ActualizarMarcaVehiculoCommand : IRequest<Unit>{
    public Guid MarcaVehiculoId { get; init; }
    public string Nombre { get; init; } = string.Empty;
}
