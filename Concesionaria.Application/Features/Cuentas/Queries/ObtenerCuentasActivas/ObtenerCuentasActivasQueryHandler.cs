using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasActivas;

public class ObtenerCuentasActivasQueryHandler : IRequestHandler<ObtenerCuentasActivasQuery, List<CuentaDto>>
{
    private readonly ICuentaRepository _repository;

    public ObtenerCuentasActivasQueryHandler(ICuentaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CuentaDto>> Handle(
        ObtenerCuentasActivasQuery request,
        CancellationToken cancellationToken
    )
    {
        var obtenerCuentasActivas = await _repository.ObtenerActivasAsync();

        return obtenerCuentasActivas
            .Select(c => new CuentaDto
            {
                CuentaId = c.Id,
                EmpresaId = c.EmpresaId,
                Nombre = c.Nombre,
                Eliminado = c.Eliminado,
            })
            .ToList();
    }
}