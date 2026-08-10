using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarTipoVehiculo;

public record AgregarTipoVehiculoCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = string.Empty;
}
