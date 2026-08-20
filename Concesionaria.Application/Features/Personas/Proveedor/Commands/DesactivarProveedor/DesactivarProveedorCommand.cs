using MediatR;
namespace Application.Features.Personas.Commands.DesactivarProveedor;

public record DesactivarProveedorCommand : IRequest<Unit>{
    public Guid ProveedorId { get; init; }
    public bool Eliminado { get; init; }
}
