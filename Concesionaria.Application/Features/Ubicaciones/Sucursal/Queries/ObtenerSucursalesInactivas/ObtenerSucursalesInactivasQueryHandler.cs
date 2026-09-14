using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Ubicaciones.Queries.ObtenerSucursalesInactivas;

public class ObtenerSucursalesInactivasQueryHandler
    : IRequestHandler<ObtenerSucursalesInactivasQuery, List<SucursalDto>>
{
    private readonly ISucursalRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerSucursalesInactivasQueryHandler(
        ISucursalRepository repository,
        ICurrentUserService currentUserService
    )
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<SucursalDto>> Handle(
        ObtenerSucursalesInactivasQuery request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUserService.EmpresaId;

        var sucursales = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

        return sucursales
            .OrderBy(s => s.Nombre)
            .Select(s => new SucursalDto
            {
                SucursalId = s.Id,
                Nombre = s.Nombre,
                Direccion = s.Direccion,
                LocalidadId = s.LocalidadId,
            })
            .ToList();
    }
}
