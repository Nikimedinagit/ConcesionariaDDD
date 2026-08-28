using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerVehiculosActivas;

public record ObtenerVehiculosActivasQuery : IRequest<List<VehiculoDto>>
{
    public string Filtro { get; set; }
}
