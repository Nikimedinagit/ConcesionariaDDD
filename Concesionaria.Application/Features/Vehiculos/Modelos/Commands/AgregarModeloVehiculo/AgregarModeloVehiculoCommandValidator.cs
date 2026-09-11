namespace Application.Features.Vehiculos.Commands.AgregarModeloVehiculo;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class AgregarModeloVehiculoCommandValidator : AbstractValidator<AgregarModeloVehiculoCommand>
{
    public AgregarModeloVehiculoCommandValidator(
        IModeloVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorNombreAsync(
                        nombre.Trim(),
                        empresaId,
                        context.InstanceToValidate.TipoVehiculoId,
                        context.InstanceToValidate.MarcaVehiculoId
                    );

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe un modelo activo con ese nombre, marca y tipo.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure(
                            "Ya existe un modelo inactivo con ese nombre, marca y tipo. Puede reactivarlo."
                        );
                }
            );

        RuleFor(cg => cg.MarcaVehiculoId)
            .NotEmpty()
            .WithMessage("La marca del vehículo es obligatoria.");

        RuleFor(cg => cg.TipoVehiculoId)
            .NotEmpty()
            .WithMessage("El tipo de vehículo es obligatorio.");
    }
}
