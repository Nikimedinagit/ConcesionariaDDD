 using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosInactivas;

 public record ObtenerModelosVehiculosInactivasQuery : IRequest<List<ModeloVehiculoDto>>
{
   public string Filtro { get; set; }
}
