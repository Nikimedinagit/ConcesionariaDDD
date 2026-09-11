using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Cuentas.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Cuentas.Commands.ActualizarCuenta;

public class ActualizarCuentaCommandValidator
    : AbstractValidator<ActualizarCuentaCommand>
{
    public ActualizarCuentaCommandValidator(
        ICuentaRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(c => c.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, cancellationToken) =>
                {
                    var estado = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre.Trim(),
                        currentUser.EmpresaId,
                        context.InstanceToValidate.CuentaId
                    );

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe una cuenta activa con ese nombre y tipo.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe una cuenta inactiva con ese nombre y tipo. Puede reactivarla.");
                }
            );
    }
}
