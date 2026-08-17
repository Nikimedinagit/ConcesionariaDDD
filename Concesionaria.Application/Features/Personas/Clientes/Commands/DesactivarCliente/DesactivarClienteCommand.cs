using MediatR;

namespace Application.Features.Personas.Commands.DesactivarCliente;

public record DesactivarClienteCommand : IRequest<Unit>{
    public Guid ClienteId { get; init; }
    public bool Eliminado { get; init; }
}
