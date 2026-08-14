using MediatR;

namespace Concesionaria.Application.Features.Personas.Proveedor.Queries.ObtenerProveedorActivas;

public record ObtenerProveedorActivasQuery : IRequest<List<ProveedorDto>>
{
    public string Filtro { get; set; }
}