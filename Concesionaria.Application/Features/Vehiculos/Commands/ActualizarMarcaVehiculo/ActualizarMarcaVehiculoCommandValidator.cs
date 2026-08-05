namespace Application.Features.Vehiculos.Commands.ActualizarMarcaVehiculo;

using System.ComponentModel.Design;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarMarcaVehiculoCommandValidator
    : AbstractValidator<ActualizarMarcaVehiculoCommand>
{
    public ActualizarMarcaVehiculoCommandValidator(
        IMarcaVehiculoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(
                async (command, nombre, cancellationToken) =>
                {
                    Console.WriteLine(command.MarcaVehiculoId);

                    var existe = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre,
                        currentUser.EmpresaId,
                        command.MarcaVehiculoId
                    );

                    Console.WriteLine(existe);

                    return !existe;
                }
            )
            .WithMessage("Ya existe esa Marca.");
    }
}
