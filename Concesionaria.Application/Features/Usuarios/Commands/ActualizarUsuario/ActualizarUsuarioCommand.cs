using MediatR;

namespace Application.Features.Usuarios.Commands.ActualizarUsuario;

public record ActualizarUsuarioCommand : IRequest<Unit>
{
    public Guid UsuarioId { get; init; }
    public string RolId { get; init; } = string.Empty;
    public Guid SucursalId { get; init; }
    public string NombreCompleto { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
