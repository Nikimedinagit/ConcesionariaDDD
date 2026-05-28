namespace Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;

using System.ComponentModel.Design;
using Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarCategoriaGastoCommandValidator : AbstractValidator<ActualizarCategoriaGastoCommand>
{
    public ActualizarCategoriaGastoCommandValidator(
        ICategoriaGastoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MustAsync(async (command, nombre, cancellationToken) =>
{
    Console.WriteLine(command.CategoriaGastoId);

    var existe = await repository.ExistePorNombreExluyendoIdAsync(
        nombre,
        currentUser.EmpresaId,
        command.CategoriaGastoId
    );

    Console.WriteLine(existe);

    return !existe;
})
            .WithMessage("Ya existe esa Categoria Gasto.");
    }
}
