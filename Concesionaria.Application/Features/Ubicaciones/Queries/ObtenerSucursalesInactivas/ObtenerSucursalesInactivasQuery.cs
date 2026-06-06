using MediatR;

namespace Application.Features.Ubicaciones.Queries.ObtenerSucursalesInactivas;

public record ObtenerSucursalesInactivasQuery : IRequest<List<SucursalDto>>
{
    public string Filtro { get; set; }
}
