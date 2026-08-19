using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Cuentas.Commands.DesactivarCuenta;

public class DesactivarCuentaCommandValidator : AbstractValidator<DesactivarCuentaCommand>
{
    public DesactivarCuentaCommandValidator(
        ICuentaRepository repository,
        ICurrentUserService currentUser)
    {
        RuleFor(c => c.Eliminado)
            .MustAsync(async (command, _, cancellationToken) =>
            {
                var tieneCuentasHijasActivas = await repository.TieneCuentasHijasActivasAsync(
                    currentUser.EmpresaId,
                    command.CuentaId);

                return !tieneCuentasHijasActivas;
            })
            .WithMessage("No se puede desactivar la cuenta porque tiene cuentas hijas activas asociadas. Primero desactive o reasigne esas cuentas.");
    }
}
