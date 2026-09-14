namespace Application.Features.Ubicaciones.Commands.ActualizarSucursal;

using Application.Features.Ubicaciones.Commands.ActualizarSucursal;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarSucursalCommandValidator : AbstractValidator<ActualizarSucursalCommand>
{
    public ActualizarSucursalCommandValidator(
        ISucursalRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, cancellationToken) =>
                {
                    var estado = await repository.ExistePorNombreLocalidadAsync(
                        nombre.Trim(),
                        currentUser.EmpresaId,
                        context.InstanceToValidate.LocalidadId,
                        context.InstanceToValidate.SucursalId
                    );

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe una sucursal activa con ese nombre y localidad.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe una sucursal inactiva con ese nombre y localidad. Puede reactivarla.");
                }
            );

        RuleFor(cg => cg.LocalidadId)
            .NotEqual(Guid.Empty)
            .WithMessage("La localidad es obligatoria.")
            .MustAsync(
                async (localidadId, cancellationToken) =>
                {
                    return await repository.LocalidadExisteAsync(localidadId);
                }
            )
            .WithMessage("La localidad no es válida.");
    }
}
