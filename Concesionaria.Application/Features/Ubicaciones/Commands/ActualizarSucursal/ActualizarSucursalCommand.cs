using MediatR;

namespace Application.Features.Ubicaciones.Commands.ActualizarSucursal;

public record ActualizarSucursalCommand : IRequest<Unit>
{
    public Guid SucursalId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Direccion { get; init; } = string.Empty;
    public Guid LocalidadId { get; init; }
}
