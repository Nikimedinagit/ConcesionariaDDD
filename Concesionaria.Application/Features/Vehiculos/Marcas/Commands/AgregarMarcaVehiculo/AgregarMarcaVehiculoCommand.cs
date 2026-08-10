using MediatR;

namespace Application.Features.Vehiculos.Commands.AgregarMarcaVehiculo;

public record AgregarMarcaVehiculoCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = string.Empty;
}
