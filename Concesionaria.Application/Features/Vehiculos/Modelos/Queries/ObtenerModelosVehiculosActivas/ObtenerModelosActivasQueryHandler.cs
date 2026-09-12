using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerModelosVehiculosActivas;

public class ObtenerModelosVehiculosActivasQueryHandler
    : IRequestHandler<ObtenerModelosVehiculosActivasQuery, List<ModeloVehiculoDto>>
{
    private readonly IModeloVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerModelosVehiculosActivasQueryHandler(IModeloVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<ModeloVehiculoDto>> Handle(
    ObtenerModelosVehiculosActivasQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var modelos = await _repository.ObtenerActivasAsync(
            empresaId,
            request.Filtro,
            request.MarcaVehiculoId,
            request.TipoVehiculoId);

        return modelos
            .OrderBy(m => m.Nombre)
            .Select(m => new ModeloVehiculoDto
            {
                ModeloVehiculoId = m.Id,
                Nombre = m.Nombre,
                MarcaVehiculoId = m.MarcaVehiculoId,
                MarcaVehiculoNombre = m.MarcaVehiculo.Nombre,
                TipoVehiculoId = m.TipoVehiculoId,
                TipoVehiculoNombre = m.TipoVehiculo.Nombre
            })
            .ToList();
    }
}
