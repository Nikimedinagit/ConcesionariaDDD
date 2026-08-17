using MediatR;

namespace Concesionaria.Application.Features.Personas.Clientes.Queries.ObtenerClientesActivas;

public record ObtenerClientesActivasQuery : IRequest<List<ClienteDto>>
{
    public string Filtro { get; set; }
}