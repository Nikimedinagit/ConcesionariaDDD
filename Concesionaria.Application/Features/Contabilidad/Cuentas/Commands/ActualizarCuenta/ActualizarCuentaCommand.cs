using Concesionaria.Domain.Cuentas.Enums;
using MediatR;

namespace Application.Features.Cuentas.Commands.ActualizarCuenta;

public record ActualizarCuentaCommand : IRequest<Unit>{
    public Guid CuentaId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    // public string? Codigo { get; init; }
    // public TipoCuenta? Tipo { get; init; }
    // public int? Nivel { get; init; }
}
