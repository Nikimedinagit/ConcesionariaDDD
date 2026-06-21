using MediatR;

namespace Application.Features.Usuarios.Commands.ActivarUsuario;

public record ActivarUsuarioCommand : IRequest<Unit>
{
    public Guid UsuarioId { get; init; }
}
