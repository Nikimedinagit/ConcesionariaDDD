namespace Application.Features.Ubicaciones.Commands.DesactivarSucursal;

using Concesionaria.Application.Common.Interfaces;
using FluentValidation;

public class DesactivarSucursalCommandValidator
    : AbstractValidator<DesactivarSucursalCommand>
{
    public DesactivarSucursalCommandValidator(
        ISucursalRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(s => s.Eliminado)
            .MustAsync(
                async (command, eliminado, cancellationToken) =>
                {
                    var tieneVehiculos = await repository.TieneVehiculosActivosAsync(
                        currentUser.EmpresaId,
                        command.SucursalId
                    );

                    return !tieneVehiculos;
                }
            )
            .WithMessage(
                "No se puede desactivar la sucursal porque tiene vehículos asociados. Reasigne esos vehículos a otra sucursal antes de continuar."
            );
    }
}