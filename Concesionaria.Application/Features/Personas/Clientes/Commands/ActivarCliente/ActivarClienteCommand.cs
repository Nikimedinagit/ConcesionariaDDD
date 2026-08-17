using MediatR;
namespace Application.Features.Personas.Commands.ActivarCliente;

public record ActivarClienteCommand : IRequest<Unit>{
    public Guid ClienteId { get; init; }
    public bool Eliminado { get; init; }
}
