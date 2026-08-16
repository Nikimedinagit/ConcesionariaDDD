using MediatR;

namespace Application.Features.Vehiculos.Commands.ActualizarModeloVehiculo;

public record ActualizarModeloVehiculoCommand : IRequest<Unit>{
    public Guid ModeloVehiculoId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public Guid MarcaVehiculoId { get; init; }
    public Guid TipoVehiculoId { get; init; }
}
