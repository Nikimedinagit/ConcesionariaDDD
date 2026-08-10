namespace Application.Features.Vehiculos.Commands.ActualizarTipoVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarTipoVehiculoCommandValidator
    : AbstractValidator<ActualizarTipoVehiculoCommand>
{
    public ActualizarTipoVehiculoCommandValidator(
        ITipoVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(
                async (command, nombre, cancellationToken) =>
                {
                    Console.WriteLine(command.TipoVehiculoId);

                    var existe = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre,
                        currentUser.EmpresaId,
                        command.TipoVehiculoId
                    );

                    Console.WriteLine(existe);

                    return !existe;
                }
            )
            .WithMessage("Ya existe el tipo de vehículo.");
    }
}
