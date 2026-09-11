namespace Application.Features.Vehiculos.Commands.AgregarMarcaVehiculo;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class AgregarMarcaVehiculoCommandValidator
    : AbstractValidator<AgregarMarcaVehiculoCommand>
{
    public AgregarMarcaVehiculoCommandValidator(
        IMarcaVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, cancellationToken) =>
                {
                    var estado = await repository.ExistePorNombreAsync(nombre.Trim(), currentUser.EmpresaId);

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe una marca activa con ese nombre.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe una marca inactiva con ese nombre. Puede reactivarla.");
                }
            );
    }
}
