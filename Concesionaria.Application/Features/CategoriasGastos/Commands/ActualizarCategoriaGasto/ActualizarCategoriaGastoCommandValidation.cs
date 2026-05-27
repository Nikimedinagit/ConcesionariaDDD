namespace Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;

using System.ComponentModel.Design;
using Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarCategoriaGastoCommandValidation
    : AbstractValidator<ActualizarCategoriaGastoCommand>
{
    public ActualizarCategoriaGastoCommandValidation(
        ICategoriaGastoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(
                async (command, nombre, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorNombreExluyendoIdAsync(
                        nombre,
                        empresaId,
                        command.CategoriaGastoId
                    );
                }
            )
            .WithMessage("Ya existe otra categoría de gasto con ese nombre.");
    }
}
