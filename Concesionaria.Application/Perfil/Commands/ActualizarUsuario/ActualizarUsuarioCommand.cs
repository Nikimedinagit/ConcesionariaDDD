using MediatR;

namespace Concesionaria.Application.Perfil.Commands.ActualizarUsuario;

public class ActualizarUsuarioCommand : IRequest<string>{
    public string UsuarioId { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? AvatarUrl { get; set; }
}