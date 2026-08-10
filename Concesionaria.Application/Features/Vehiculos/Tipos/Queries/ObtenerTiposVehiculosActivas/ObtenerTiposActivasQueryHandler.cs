using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerTiposVehiculosActivas;

public class ObtenerTiposVehiculosActivasQueryHandler
    : IRequestHandler<ObtenerTiposVehiculosActivasQuery, List<TipoVehiculoDto>>
{
    private readonly ITipoVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerTiposVehiculosActivasQueryHandler(ITipoVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<TipoVehiculoDto>> Handle(
    ObtenerTiposVehiculosActivasQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var tipos = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

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
