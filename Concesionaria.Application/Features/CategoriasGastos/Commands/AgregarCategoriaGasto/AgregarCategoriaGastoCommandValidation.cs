namespace Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;

using FluentValidation;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Application.Common.Interfaces;

public class AgregarCategoriaGastoCommandValidation
    : AbstractValidator<AgregarCategoriaGastoCommand>
{
    public AgregarCategoriaGastoCommandValidation(
        ICategoriaGastoRepository repository,
        ICurrentUserService currentUser)
    {
        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(async (nombre, cancellationToken) =>
            {
                var empresaId = currentUser.EmpresaId;

                return !await repository.ExistePorNombreAsync(nombre, empresaId);
            })
            .WithMessage("Ya existe una categoría de gasto con ese nombre.");
    }
}
