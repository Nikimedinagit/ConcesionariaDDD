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
        CancellationToken cancellationToken
    )
    {
        var obtenerCategoriasGastosInactivas = await _repository.ObtenerInactivasAsync();

        return obtenerCategoriasGastosInactivas
            .Select(cg => new CategoriasGastosDto
            {
                CategoriaGastoId = cg.Id,
                EmpresaId = cg.EmpresaId,
                Nombre = cg.Nombre,
                Eliminado = cg.Eliminado,
            })
            .ToList();
    }
}
