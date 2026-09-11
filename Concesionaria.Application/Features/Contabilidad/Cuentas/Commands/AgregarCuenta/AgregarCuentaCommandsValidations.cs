using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Cuentas.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Cuentas.Commands.AgregarCuenta;
public class AgregarCuentaCommandValidation : AbstractValidator<AgregarCuentaCommand>
{
    public AgregarCuentaCommandValidation(
        ICuentaRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(c => c.Codigo)
            .NotEmpty()
            .WithMessage("El código es obligatorio.")
            .CustomAsync(
                async (codigo, context, cancellationToken) =>
                {
                    var estado = await repository.ExistePorCodigoAsync(codigo.Trim(), currentUser.EmpresaId);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe una cuenta activa con ese código.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe una cuenta inactiva con ese código. Puede reactivarla.");
                }
            );
            
        RuleFor(c => c.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, cancellationToken) =>
                {
                    var estado = await repository.ExistePorNombreAsync(
                        nombre.Trim(),
                        currentUser.EmpresaId,
                        context.InstanceToValidate.Tipo);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe una cuenta activa con ese nombre y tipo.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe una cuenta inactiva con ese nombre y tipo. Puede reactivarla.");
                }
            );
    }
}
