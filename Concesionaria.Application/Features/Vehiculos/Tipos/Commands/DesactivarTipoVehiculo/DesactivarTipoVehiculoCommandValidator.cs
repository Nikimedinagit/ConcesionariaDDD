
namespace Application.Features.Vehiculos.Commands.DesactivarTipoVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class DesactivarTipoVehiculoCommandValidator
    : AbstractValidator<DesactivarTipoVehiculoCommand>
{
    public DesactivarTipoVehiculoCommandValidator(
        ITipoVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Eliminado)
            .MustAsync(
                async (command, eliminado, cancellationToken) =>
                {
                    Console.WriteLine(command.TipoVehiculoId);

                    var existe = await repository.TieneModelosActivosAsync(
                        currentUser.EmpresaId,
                        command.TipoVehiculoId
                    );

                    Console.WriteLine(existe);

                    return !existe;
                }
            )
            .WithMessage("El tipo seleccionado no se puede desactivar porque tiene modelos asociados.");
    }
}
