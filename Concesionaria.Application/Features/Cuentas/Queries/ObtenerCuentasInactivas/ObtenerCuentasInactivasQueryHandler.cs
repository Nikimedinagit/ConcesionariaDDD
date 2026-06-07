using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasInactivas;

public class ObtenerCuentasInactivasQueryHandler
    : IRequestHandler<ObtenerCuentasInactivasQuery, List<CuentaDto>>
{
    private readonly ICuentaRepository _repository;

    public ObtenerCuentasInactivasQueryHandler(ICuentaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CuentaDto>> Handle(
        ObtenerCuentasInactivasQuery request,
        CancellationToken cancellationToken)
    {
        var cuentas = await _repository.ObtenerInactivasAsync();

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