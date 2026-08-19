namespace Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;

using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Interfaces.IRepositories;
using FluentValidation;

public class AgregarCategoriaGastoCommandValidator
    : AbstractValidator<AgregarCategoriaGastoCommand>
{
    public AgregarCategoriaGastoCommandValidator(
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
                    var estado = await repository.ExistePorNombreAsync(nombre.Trim(), currentUser.EmpresaId);

                    if (estado == NombreCategoriaGastoEstado.Activo)
                        context.AddFailure("Ya existe una categoría de gasto activa con ese nombre.");

                    if (estado == NombreCategoriaGastoEstado.Desactivado)
                        context.AddFailure("Ya existe una categoría de gasto inactiva con ese nombre. Puede reactivarla.");
                }
            );
    }
}
