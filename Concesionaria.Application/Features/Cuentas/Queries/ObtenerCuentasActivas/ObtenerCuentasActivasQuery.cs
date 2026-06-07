using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasActivas;

public record ObtenerCuentasActivasQuery : IRequest<List<CuentaDto>>
{
    public string Filtro { get; set; }
}
