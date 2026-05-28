using MediatR;

namespace Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;

public record ObtenerCategoriasGastosInactivasQuery : IRequest<List<CategoriasGastosDto>>
{
    public string Filtro { get; set; }
}
