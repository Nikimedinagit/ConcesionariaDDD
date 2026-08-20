using MediatR;

namespace Concesionaria.Application.Features.Personas.Proveedor.Queries.ObtenerProveedorInactivas;

public record ObtenerProveedorInactivasQuery : IRequest<List<ProveedorDto>>
{
    public string Filtro { get; set; }
}