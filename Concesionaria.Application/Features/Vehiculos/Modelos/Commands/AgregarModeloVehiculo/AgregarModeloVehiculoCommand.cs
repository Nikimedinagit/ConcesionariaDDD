using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarModeloVehiculo;

public record AgregarModeloVehiculoCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = string.Empty;
    public Guid MarcaVehiculoId { get; init; }
    public Guid TipoVehiculoId { get; init; }
}
