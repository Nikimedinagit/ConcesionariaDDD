 using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerMarcasVehiculosInactivas;

 public record ObtenerMarcasVehiculosInactivasQuery : IRequest<List<MarcaVehiculoDto>>
{
   public string Filtro { get; set; }
}
