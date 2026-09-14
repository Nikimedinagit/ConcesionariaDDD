using MediatR;

namespace Application.Features.Ubicaciones.Queries.ObtenerSucursalesActivas;

public record ObtenerSucursalesActivasQuery : IRequest<List<SucursalDto>>
{
    public string Filtro { get; set; }
}
