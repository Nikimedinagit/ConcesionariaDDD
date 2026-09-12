
namespace Application.Features.Vehiculos.Commands.DesactivarModeloVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class DesactivarModeloVehiculoCommandValidator
    : AbstractValidator<DesactivarModeloVehiculoCommand>
{
    public DesactivarModeloVehiculoCommandValidator(
        IModeloVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Eliminado)
            .MustAsync(
                async (command, eliminado, cancellationToken) =>
                {
                    var existe = await repository.TieneVehiculosActivosAsync(
                        currentUser.EmpresaId,
                        command.ModeloVehiculoId
                    );

                    return !existe;
                }
            )
            .WithMessage("No se puede desactivar el modelo porque tiene vehículos asociados.");
    }
}
