using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using MediatR;

namespace Application.Features.Ubicaciones.Queries.ObtenerSucursalesActivas;

public class ObtenerSucursalesActivasQueryHandler
    : IRequestHandler<ObtenerSucursalesActivasQuery, List<SucursalDto>>
{
    private readonly ISucursalRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerSucursalesActivasQueryHandler(
        ISucursalRepository repository,
        ICurrentUserService currentUserService
    )
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<SucursalDto>> Handle(
        ObtenerSucursalesActivasQuery request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUserService.EmpresaId;

        var sucursales = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

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
