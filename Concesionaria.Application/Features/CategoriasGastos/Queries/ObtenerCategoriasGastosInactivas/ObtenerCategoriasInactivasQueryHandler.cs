using Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosInactivas;

public class ObtenerCategoriasGastosInactivasQueryHandler
    : IRequestHandler<ObtenerCategoriasGastosInactivasQuery, List<CategoriasGastosDto>>
{
    private readonly ICategoriaGastoRepository _repository;

    public ObtenerCategoriasGastosInactivasQueryHandler(ICategoriaGastoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoriasGastosDto>> Handle(
        ObtenerCategoriasGastosInactivasQuery request,
        CancellationToken cancellationToken)
    {
        var categorias = await _repository.ObtenerInactivasAsync(request.Filtro);

        return categorias
            .OrderBy(cg => cg.Nombre)
            .Select(cg => new CategoriasGastosDto
            {
                CategoriaGastoId = cg.Id,
                Nombre = cg.Nombre
            })
            .ToList();
    }
}

