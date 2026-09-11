 using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosEnReparacion;

 public record ObtenerVehiculosEnReparacionQuery : IRequest<List<VehiculoDto>>
{
   public string Filtro { get; set; }
}
