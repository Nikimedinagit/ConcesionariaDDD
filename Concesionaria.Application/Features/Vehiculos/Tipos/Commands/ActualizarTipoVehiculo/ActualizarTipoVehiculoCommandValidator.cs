namespace Application.Features.Vehiculos.Commands.ActualizarTipoVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarTipoVehiculoCommandValidator
    : AbstractValidator<ActualizarTipoVehiculoCommand>
{
    public ActualizarTipoVehiculoCommandValidator(
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
                    var estado = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre.Trim(),
                        currentUser.EmpresaId,
                        context.InstanceToValidate.TipoVehiculoId
                    );

                    if (estado == NombreEntidadVehiculoEstado.Activo)
                        context.AddFailure("Ya existe un tipo de vehículo activo con ese nombre.");

                    if (estado == NombreEntidadVehiculoEstado.Desactivado)
                        context.AddFailure("Ya existe un tipo de vehículo inactivo con ese nombre. Puede reactivarlo.");
                }
            );
    }
}
