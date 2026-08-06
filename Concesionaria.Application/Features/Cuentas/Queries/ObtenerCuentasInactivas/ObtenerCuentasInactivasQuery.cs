using MediatR;
using Concesionaria.Domain.Cuentas.Enums;

namespace Application.Features.Cuentas.Queries.ObtenerCuentasInactivas;

public record ObtenerCuentasInactivasQuery : IRequest<List<CuentaDto>>
{
    public string Filtro { get; set; }
    public TipoCuenta? Tipo { get; set; }
    public int? Nivel { get; set; }
}
