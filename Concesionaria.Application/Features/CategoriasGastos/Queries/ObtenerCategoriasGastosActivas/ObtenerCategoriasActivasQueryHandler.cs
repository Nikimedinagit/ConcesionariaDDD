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
        CancellationToken cancellationToken
    )
    {
        var obtenerCategoriasGastosActivas = await _repository.ObtenerActivasAsync();

        return obtenerCategoriasGastosActivas
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
