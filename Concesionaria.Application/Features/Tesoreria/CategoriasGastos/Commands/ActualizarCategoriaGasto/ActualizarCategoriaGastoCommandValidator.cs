namespace Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;

using System.ComponentModel.Design;
using Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class ActualizarCategoriaGastoCommandValidator
    : AbstractValidator<ActualizarCategoriaGastoCommand>
{
    public ActualizarCategoriaGastoCommandValidator(
        ICategoriaGastoRepository repository,
        ICurrentUserService currentUser
    )
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .CustomAsync(
                async (nombre, context, cancellationToken) =>
                {
                    var estado = await repository.ExistePorNombreExluyendoIdAsync(
                        nombre.Trim(),
                        currentUser.EmpresaId,
                        context.InstanceToValidate.CategoriaGastoId
                    );

                    if (estado == EstadoExistencia.Activo)
                        context.AddFailure("Ya existe una categoría de gasto activa con ese nombre.");

                    if (estado == EstadoExistencia.Desactivado)
                        context.AddFailure("Ya existe una categoría de gasto inactiva con ese nombre. Puede reactivarla.");
                }
            );
    }
}
