using MediatR;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasInactivas;

public record ObtenerCuentasInactivasQuery : IRequest<List<CuentaDto>>
{
    public string Filtro { get; set; }
}
