using MediatR;

namespace Application.Features.Ubicaciones.Commands.AgregarSucursal;

public record AgregarSucursalCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = string.Empty;
    public string Direccion { get; init; } = string.Empty;
    public Guid LocalidadId { get; init; }
}
