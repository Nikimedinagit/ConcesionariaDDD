namespace Application.Features.Vehiculos.Commands.AgregarModeloVehiculo;

using Concesionaria.Application.Common.Interfaces;
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
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, ct) =>
                {
                    var empresaId = currentUser.EmpresaId;
                    var estado = await repository.ExistePorNombreAsync(
                        nombre.Trim(),
                        empresaId
                    );

                    if (estado == NombreModeloEstado.Activo)
                        context.AddFailure("Ya existe ese modelo en activos.");

                    if (estado == NombreModeloEstado.Desactivado)
                        context.AddFailure(
                            "Ese modelo ya existe pero está desactivado. Puede reactivarlo."
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
