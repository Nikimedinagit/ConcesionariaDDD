using Concesionaria.Domain.Cuentas.Enums;
using MediatR;

namespace Application.Features.Vehiculos.Commands.EliminarVehiculo;

public record EliminarVehiculoCommand : IRequest<Unit>
{
    public Guid VehiculoId { get; set; }
}
