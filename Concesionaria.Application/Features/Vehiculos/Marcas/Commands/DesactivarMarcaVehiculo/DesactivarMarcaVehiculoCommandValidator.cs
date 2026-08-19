
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
                    var existe = await repository.TieneModelosActivosAsync(
                        currentUser.EmpresaId,
                        command.MarcaVehiculoId
                    );

                    return !existe;
                }
            )
            .WithMessage("No se puede desactivar la marca porque tiene modelos activos asociados. Primero desactive o reasigne esos modelos.");
    }
}
