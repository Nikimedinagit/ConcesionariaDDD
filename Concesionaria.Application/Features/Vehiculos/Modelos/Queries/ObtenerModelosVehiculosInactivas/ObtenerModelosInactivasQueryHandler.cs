using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosInactivas;

public class ObtenerModelosVehiculosInactivasQueryHandler
    : IRequestHandler<ObtenerModelosVehiculosInactivasQuery, List<ModeloVehiculoDto>>
{
    private readonly IModeloVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerModelosVehiculosInactivasQueryHandler(
        IModeloVehiculoRepository repository,
        ICurrentUserService currentUserService
    )
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<ModeloVehiculoDto>> Handle(
        ObtenerModelosVehiculosInactivasQuery request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUserService.EmpresaId;
        var modelos = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

        return modelos
            .OrderBy(m => m.Nombre)
            .Select(m => new ModeloVehiculoDto
            {
                ModeloVehiculoId = m.Id,
                Nombre = m.Nombre,
                MarcaVehiculoId = m.MarcaVehiculoId,
                TipoVehiculoId = m.TipoVehiculoId,
            })
            .ToList();
    }
}
