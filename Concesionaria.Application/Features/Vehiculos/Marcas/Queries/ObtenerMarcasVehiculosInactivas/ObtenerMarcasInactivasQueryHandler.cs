using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Vehiculos.Queries.ObtenerMarcasVehiculosInactivas;

public class ObtenerMarcasVehiculosInactivasQueryHandler
    : IRequestHandler<ObtenerMarcasVehiculosInactivasQuery, List<MarcaVehiculoDto>>
{
    private readonly IMarcaVehiculoRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerMarcasVehiculosInactivasQueryHandler(IMarcaVehiculoRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<MarcaVehiculoDto>> Handle(
        ObtenerMarcasVehiculosInactivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;
        var marcas = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

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

