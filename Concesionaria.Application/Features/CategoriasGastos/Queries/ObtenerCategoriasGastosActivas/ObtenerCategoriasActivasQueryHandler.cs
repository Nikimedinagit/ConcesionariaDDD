using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;

public class ObtenerCategoriasGastosActivasQueryHandler
    : IRequestHandler<ObtenerCategoriasGastosActivasQuery, List<CategoriasGastosDto>>
{
    private readonly ICategoriaGastoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerCategoriasGastosActivasQueryHandler(ICategoriaGastoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<CategoriasGastosDto>> Handle(
    ObtenerCategoriasGastosActivasQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var categorias = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

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
