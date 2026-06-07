using Concesionaria.Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasActivas;

public class ObtenerCuentasActivasQueryHandler
    : IRequestHandler<ObtenerCuentasActivasQuery, List<CuentaDto>>
{
    private readonly ICuentaRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public ObtenerCuentasActivasQueryHandler(ICuentaRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<List<CuentaDto>> Handle(
        ObtenerCuentasActivasQuery request,
        CancellationToken cancellationToken)
    {
        var empresaId = _currentUserService.EmpresaId;

        var cuentas = await _repository.ObtenerActivasAsync(empresaId, request.Filtro);

        return cuentas
            .Select(c => new CuentaDto
            {
                CuentaId = c.Id,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Tipo = c.Tipo,
                Nivel = c.Nivel
            })
            .OrderBy(c => c.Nivel).ThenBy(c => c.Tipo).ThenBy(c => c.Codigo)
            .ToList();
    }
}