using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerMarcasVehiculosActivas;

public record ObtenerMarcasVehiculosActivasQuery : IRequest<List<MarcaVehiculoDto>>
{
    public string Filtro { get; set; }
}
