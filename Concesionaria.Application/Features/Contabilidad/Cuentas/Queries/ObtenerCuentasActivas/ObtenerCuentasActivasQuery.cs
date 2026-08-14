using MediatR;
using Concesionaria.Domain.Cuentas.Enums;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasActivas;

public record ObtenerCuentasActivasQuery : IRequest<List<CuentaDto>>
{
    public string Filtro { get; set; }
    public TipoCuenta? Tipo { get; set; }
    public int? Nivel { get; set; }
}
