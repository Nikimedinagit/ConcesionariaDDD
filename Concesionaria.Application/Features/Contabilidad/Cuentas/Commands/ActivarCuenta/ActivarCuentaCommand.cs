using MediatR;

namespace Application.Features.Cuentas.Commands.ActivarCuenta;

public record ActivarCuentaCommand : IRequest<Unit>{
    public Guid CuentaId { get; init; }
    public bool Eliminado { get; init; }
}
