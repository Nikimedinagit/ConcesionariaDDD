using MediatR;

namespace Application.Features.Ubicaciones.Commands.ActivarSucursal;

public record ActivarSucursalCommand : IRequest<Unit>{
    public Guid SucursalId { get; init; }
    public bool Eliminado { get; init; }
}