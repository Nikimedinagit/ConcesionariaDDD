using MediatR;

namespace Application.Features.Ubicaciones.Commands.DesactivarSucursal;

public record DesactivarSucursalCommand : IRequest<Unit>{
    public Guid SucursalId { get; init; }
    public bool Eliminado { get; init; }
}