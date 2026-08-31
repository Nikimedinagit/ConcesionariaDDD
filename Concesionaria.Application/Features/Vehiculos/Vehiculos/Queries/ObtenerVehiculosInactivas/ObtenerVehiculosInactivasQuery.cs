 using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosInactivas;

 public record ObtenerVehiculosInactivasQuery : IRequest<List<VehiculoDto>>
{
   public string Filtro { get; set; }
}
