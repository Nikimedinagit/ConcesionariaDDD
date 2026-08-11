namespace Application.Features.Vehiculos.Commands.AgregarTipoVehiculo;

using Concesionaria.Application.Common.Interfaces;
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
            .MustAsync(
                async (nombre, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorNombreAsync(nombre, empresaId);
                }
            )
            .WithMessage("Ya existe ese Tipo de Vehículo.");
    }
}
