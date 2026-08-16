
namespace Application.Features.Vehiculos.Commands.DesactivarMarcaVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class DesactivarMarcaVehiculoCommandValidator
    : AbstractValidator<DesactivarMarcaVehiculoCommand>
{
    public DesactivarMarcaVehiculoCommandValidator(
        IMarcaVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Eliminado)
            .MustAsync(
                async (command, eliminado, cancellationToken) =>
                {
                    Console.WriteLine(command.MarcaVehiculoId);

                    var existe = await repository.TieneModelosActivosAsync(
                        currentUser.EmpresaId,
                        command.MarcaVehiculoId
                    );

                    Console.WriteLine(existe);

                    return !existe;
                }
            )
            .WithMessage("La marca seleccionada no se puede desactivar porque tiene modelos asociados.");
    }
}
