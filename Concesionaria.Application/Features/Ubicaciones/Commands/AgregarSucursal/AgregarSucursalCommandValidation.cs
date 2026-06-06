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
            .MustAsync(
                async (nombre, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorNombreLocalidadAsync(nombre, empresaId);
                }
            )
            .WithMessage("Ya existe la sucursal.");

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
