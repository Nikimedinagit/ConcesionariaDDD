using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.DTOs.Usuarios;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Usuarios.Queries.ObtenerUsuariosActivos;

public class ObtenerUsuariosActivosQueryHandler
    : IRequestHandler<ObtenerUsuariosActivosQuery, List<UsuarioDto>>
{
    private readonly IUsuarioRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public ObtenerUsuariosActivosQueryHandler(
        IUsuarioRepository repository,
        ICurrentUserService currentUser
    )
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<List<UsuarioDto>> Handle(
        ObtenerUsuariosActivosQuery request,
        CancellationToken cancellationToken
    )
    {
        var usuarios = await _repository.ObtenerActivosAsync(
            _currentUser.EmpresaId,
            request.Filtro
        );
        var roles = await _repository.ObtenerRolesAsync();

        return usuarios
            .OrderBy(u => u.NombreCompleto)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.Id,
                EmpresaId = u.EmpresaId,
                RolId = u.RolId,
                RolNombre = roles.GetValueOrDefault(u.RolId, u.RolId),
                SucursalId = u.SucursalId,
                SucursalNombre = u.Sucursal?.Nombre ?? string.Empty,
                NombreCompleto = u.NombreCompleto,
                Email = u.Email,
                FechaAlta = u.FechaAlta,
                UltimoAcceso = u.UltimoAcceso,
                Estado = u.Estado.ToString(),
            })
            .ToList();
    }
}
