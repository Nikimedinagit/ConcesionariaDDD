using Concesionaria.Application.DTOs.Usuarios;
using MediatR;

namespace Application.Features.Usuarios.Queries.ObtenerUsuariosInactivos;

public record ObtenerUsuariosInactivosQuery : IRequest<List<UsuarioDto>>
{
    public string Filtro { get; init; } = string.Empty;
}
