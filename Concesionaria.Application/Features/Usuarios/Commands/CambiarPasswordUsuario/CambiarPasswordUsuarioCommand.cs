using MediatR;

namespace Application.Features.Usuarios.Commands.CambiarPasswordUsuario;

public record CambiarPasswordUsuarioCommand : IRequest<Unit>
{
    public Guid UsuarioId { get; init; }
    public string Password { get; init; } = string.Empty;
}
