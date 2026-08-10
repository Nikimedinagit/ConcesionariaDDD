using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosInactivas;

public class ObtenerTiposVehiculosInactivasQueryHandler
    : IRequestHandler<ObtenerTiposVehiculosInactivasQuery, List<TipoVehiculoDto>>
{
    private readonly ITipoVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerTiposVehiculosInactivasQueryHandler(ITipoVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<TipoVehiculoDto>> Handle(
    ObtenerTiposVehiculosInactivasQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var tipos = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

        return tipos
            .OrderBy(t => t.Nombre)
            .Select(t => new TipoVehiculoDto
            {
                TipoVehiculoId = t.Id,
                Nombre = t.Nombre
            })
            .ToList();
    }
}
