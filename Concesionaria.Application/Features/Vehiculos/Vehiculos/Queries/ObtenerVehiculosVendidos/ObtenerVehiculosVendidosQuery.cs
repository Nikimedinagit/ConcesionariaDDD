 using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosVendidos;

 public record ObtenerVehiculosVendidosQuery : IRequest<List<VehiculoDto>>
{
   public string Filtro { get; set; }
   public Guid? SucursalId { get; set; }
   public bool TodasSucursales { get; set; }
}
