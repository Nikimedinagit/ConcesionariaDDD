using Concesionaria.Application.DTOs.Usuarios;
using MediatR;

namespace Application.Features.Usuarios.Queries.ObtenerUsuariosActivos;

public record ObtenerUsuariosActivosQuery : IRequest<List<UsuarioDto>>
{
    public string Filtro { get; init; } = string.Empty;
}
