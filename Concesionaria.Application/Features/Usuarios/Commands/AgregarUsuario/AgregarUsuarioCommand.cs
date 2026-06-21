using MediatR;

namespace Application.Features.Usuarios.Commands.AgregarUsuario;

public record AgregarUsuarioCommand : IRequest<Guid>
{
    public string RolId { get; init; } = string.Empty;
    public Guid SucursalId { get; init; }
    public string NombreCompleto { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
