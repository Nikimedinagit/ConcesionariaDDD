
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
                    var existe = await repository.TieneModelosActivosAsync(
                        currentUser.EmpresaId,
                        command.TipoVehiculoId
                    );

                    return !existe;
                }
            )
            .WithMessage("No se puede desactivar el tipo de vehículo porque tiene modelos activos asociados. Primero desactive o reasigne esos modelos.");
    }
}
