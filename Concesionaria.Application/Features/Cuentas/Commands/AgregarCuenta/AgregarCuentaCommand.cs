using Concesionaria.Domain.Cuentas.Enums;
using MediatR;

namespace Application.Features.Cuentas.Commands.AgregarCuenta;

public record AgregarCuentaCommand : IRequest<Guid>
{
    public string Nombre { get; init; } = string.Empty;
    public string Codigo { get; init; } = string.Empty;
    public int Nivel { get; init; }
    public TipoCuenta Tipo { get; init; }
    public Guid? CuentaPadreId { get; init; }
}
