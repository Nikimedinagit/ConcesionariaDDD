using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosActivas;

public record ObtenerTiposVehiculosActivasQuery : IRequest<List<TipoVehiculoDto>>
{
    public string Filtro { get; set; }
}
