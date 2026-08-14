using MediatR;

namespace Application.Features.Cuentas.Commands.DesactivarCuenta;

public record DesactivarCuentaCommand : IRequest<Unit>{
    public Guid CuentaId { get; init; }
    public bool Eliminado { get; init; }
}