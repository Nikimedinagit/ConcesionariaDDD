using MediatR;
namespace Application.Features.Personas.Commands.ActivarProveedor;

public record ActivarProveedorCommand : IRequest<Unit>{
    public Guid ProveedorId { get; init; }
    public bool Eliminado { get; init; }
}
