namespace Application.Features.Vehiculos.Commands.ActualizarModeloVehiculo;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarModeloVehiculoCommandValidator
    : AbstractValidator<ActualizarModeloVehiculoCommand>
{
    public ActualizarModeloVehiculoCommandValidator(
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
                    var estado = await repository.ExistePorNombreTipoMarcaExluyendoIdAsync(nombre.Trim(), empresaId, context.InstanceToValidate.ModeloVehiculoId, context.InstanceToValidate.TipoVehiculoId, context.InstanceToValidate.MarcaVehiculoId);

                    if (estado == NombreModeloEstado.Activo)
                        context.AddFailure("Ya existe un modelo activo con ese nombre, marca y tipo.");

                    if (estado == NombreModeloEstado.Desactivado)
                        context.AddFailure(
                            "Ya existe un modelo inactivo con ese nombre, marca y tipo. Puede reactivarlo"
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
