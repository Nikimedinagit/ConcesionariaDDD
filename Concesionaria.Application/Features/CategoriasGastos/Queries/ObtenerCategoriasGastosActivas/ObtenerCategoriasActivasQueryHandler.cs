using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;

public class ObtenerCategoriasGastosActivasQueryHandler
    : IRequestHandler<ObtenerCategoriasGastosActivasQuery, List<CategoriasGastosDto>>
{
    private readonly ICategoriaGastoRepository _repository;

    public ObtenerCategoriasGastosActivasQueryHandler(ICategoriaGastoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoriasGastosDto>> Handle(
    ObtenerCategoriasGastosActivasQuery request,
    CancellationToken cancellationToken)
{
    var categorias = await _repository.ObtenerActivasAsync();

    return categorias
        .Select(cg => new CategoriasGastosDto
        {
            CategoriaGastoId = cg.Id,
            Nombre = cg.Nombre
        })
        .ToList();
}
}
