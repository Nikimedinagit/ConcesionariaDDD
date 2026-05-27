using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasActivas;

public class ObtenerCuentasActivasQueryHandler
    : IRequestHandler<ObtenerCuentasActivasQuery, List<CuentaDto>>
{
    private readonly ICuentaRepository _repository;

    public ObtenerCuentasActivasQueryHandler(ICuentaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CuentaDto>> Handle(
        ObtenerCuentasActivasQuery request,
        CancellationToken cancellationToken)
    {
        var cuentas = await _repository.ObtenerActivasAsync();

        return cuentas
            .Select(c => new CuentaDto
            {
                CuentaId = c.Id,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Tipo = c.Tipo,
                Nivel = c.Nivel
            })
            .ToList();
    }
}