 using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosEnServicio;

 public record ObtenerVehiculosEnServicioQuery : IRequest<List<VehiculoDto>>
{
   public string Filtro { get; set; }
}
