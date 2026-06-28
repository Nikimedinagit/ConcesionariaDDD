using System.ComponentModel.Design;
using Application.Features.Cuentas.Commands.AgregarCuenta;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

namespace Application.Features.Cuentas.Commands.ActualizarCuenta;

public class ActualizarCuentaCommandValidator
    : AbstractValidator<ActualizarCuentaCommand>
{
    public ActualizarCuentaCommandValidator(
        ICuentaRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(c => c.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(
                async (command, nombre, cancellationToken) =>
                {
                    Console.WriteLine(command.CuentaId);

                    var existe = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre,
                        currentUser.EmpresaId,
                        command.CuentaId
                    );

                    Console.WriteLine(existe);

                    return !existe;
                }
            )
            .WithMessage("Ya existe esa Cuenta.");
    }
}
