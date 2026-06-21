namespace Application.Features.Ubicaciones.Commands.ActualizarSucursal;

using Application.Features.Ubicaciones.Commands.ActualizarSucursal;
using Concesionaria.Application.Common.Interfaces;
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
            .MustAsync(
                async (command, nombre, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorNombreLocalidadAsync(
                        nombre,
                        empresaId,
                        command.SucursalId
                    );
                }
            )
            .WithMessage("Ya existe la sucursal.");

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
