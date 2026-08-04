using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerMarcasVehiculosActivas;

public class ObtenerMarcasVehiculosActivasQueryHandler
    : IRequestHandler<ObtenerMarcasVehiculosActivasQuery, List<MarcaVehiculoDto>>
{
    private readonly IMarcaVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;


    public ObtenerMarcasVehiculosActivasQueryHandler(IMarcaVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<MarcaVehiculoDto>> Handle(
    ObtenerMarcasVehiculosActivasQuery request,
    CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var marcas = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

        return marcas
            .OrderBy(m => m.Nombre)
            .Select(m => new MarcaVehiculoDto
            {
                MarcaVehiculoId = m.Id,
                Nombre = m.Nombre
            })
            .ToList();
    }
}
