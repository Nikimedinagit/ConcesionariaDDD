using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosActivas;

public record ObtenerModelosVehiculosActivasQuery : IRequest<List<ModeloVehiculoDto>>
{
    public string Filtro { get; set; }
    public Guid? MarcaVehiculoId { get; set; }
    public Guid? TipoVehiculoId { get; set; }
}
