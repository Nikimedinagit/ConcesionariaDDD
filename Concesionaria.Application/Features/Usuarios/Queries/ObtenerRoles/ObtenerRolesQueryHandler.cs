using Concesionaria.Application.DTOs.Usuarios;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Usuarios.Queries.ObtenerRoles;

public class ObtenerRolesQueryHandler : IRequestHandler<ObtenerRolesQuery, List<RolDto>>
{
    private readonly IUsuarioRepository _repository;

    public ObtenerRolesQueryHandler(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RolDto>> Handle(
        ObtenerRolesQuery request,
        CancellationToken cancellationToken
    )
    {
        var roles = await _repository.ObtenerRolesAsync();

        return roles
            .OrderBy(r => r.Value)
            .Select(r => new RolDto
            {
                RolId = r.Key,
                Nombre = r.Value,
            })
            .ToList();
    }
}
