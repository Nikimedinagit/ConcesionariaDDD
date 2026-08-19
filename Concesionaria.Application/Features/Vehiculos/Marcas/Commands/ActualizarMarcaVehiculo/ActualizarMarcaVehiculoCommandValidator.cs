namespace Application.Features.Vehiculos.Commands.ActualizarMarcaVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarMarcaVehiculoCommandValidator
    : AbstractValidator<ActualizarMarcaVehiculoCommand>
{
    public ActualizarMarcaVehiculoCommandValidator(
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
                    var estado = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre.Trim(),
                        currentUser.EmpresaId,
                        context.InstanceToValidate.MarcaVehiculoId
                    );

                    if (estado == NombreEntidadVehiculoEstado.Activo)
                        context.AddFailure("Ya existe una marca activa con ese nombre.");

                    if (estado == NombreEntidadVehiculoEstado.Desactivado)
                        context.AddFailure("Ya existe una marca inactiva con ese nombre. Puede reactivarla.");
                }
            );
    }
}
