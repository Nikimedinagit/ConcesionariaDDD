using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasInactivas;

public class ObtenerCuentasInactivasQueryHandler
    : IRequestHandler<ObtenerCuentasInactivasQuery, List<CuentaDto>>
{
    private readonly ICuentaRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerCuentasInactivasQueryHandler(ICuentaRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<CuentaDto>> Handle(
        ObtenerCuentasInactivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var cuentas = await _repository.ObtenerInactivasAsync(
            empresaId,
            request.Filtro,
            request.Tipo,
            request.Nivel);

        return cuentas
            .Select(c => new CuentaDto
            {
                CuentaId = c.Id,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Tipo = c.Tipo,
                Nivel = c.Nivel,
                CuentaPadreId = c.CuentaPadreId
            })
            .OrderBy(c => c.Codigo)
            .ToList();
    }
}
