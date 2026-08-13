using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosActivas;

public record ObtenerModelosVehiculosActivasQuery : IRequest<List<ModeloVehiculoDto>>
{
    public string Filtro { get; set; }
}
