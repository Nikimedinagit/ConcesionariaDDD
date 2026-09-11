namespace Application.Features.Vehiculos.Commands.AgregarTipoVehiculo;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class AgregarTipoVehiculoCommandValidator
    : AbstractValidator<AgregarTipoVehiculoCommand>
{
    public AgregarTipoVehiculoCommandValidator(
        ITipoVehiculoRepository repository,
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
                        context.AddFailure("Ya existe un tipo de vehículo activo con ese nombre.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe un tipo de vehículo inactivo con ese nombre. Puede reactivarlo.");
                }
            );
    }
}
