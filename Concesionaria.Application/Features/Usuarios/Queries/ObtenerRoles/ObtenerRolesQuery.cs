using Concesionaria.Application.DTOs.Usuarios;
using MediatR;

namespace Application.Features.Usuarios.Queries.ObtenerRoles;

public record ObtenerRolesQuery : IRequest<List<RolDto>>;
