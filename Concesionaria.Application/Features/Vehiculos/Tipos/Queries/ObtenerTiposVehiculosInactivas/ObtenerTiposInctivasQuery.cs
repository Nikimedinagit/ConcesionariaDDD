using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosInactivas;

public record ObtenerTiposVehiculosInactivasQuery : IRequest<List<TipoVehiculoDto>>
{
    public string Filtro { get; set; }
}
