namespace Application.Features.Ubicaciones.Commands.AgregarSucursal;

using Application.Features.Ubicaciones.Commands.AgregarSucursal;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class AgregarSucursalCommandValidator : AbstractValidator<AgregarSucursalCommand>
{
    public AgregarSucursalCommandValidator(
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
                        context.InstanceToValidate.LocalidadId
                    );

                    if (estado == NombreSucursalEstado.Activo)
                        context.AddFailure("Ya existe una sucursal activa con ese nombre y localidad.");

                    if (estado == NombreSucursalEstado.Desactivado)
                        context.AddFailure("Ya existe una sucursal inactiva con ese nombre y localidad. Puede reactivarla.");
                }
            );

        RuleFor(cg => cg.LocalidadId)
            .NotEmpty()
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
