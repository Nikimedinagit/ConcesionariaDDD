using MediatR;

namespace Application.Features.Usuarios.Commands.DesactivarUsuario;

public record DesactivarUsuarioCommand : IRequest<Unit>
{
    public Guid UsuarioId { get; init; }
}
