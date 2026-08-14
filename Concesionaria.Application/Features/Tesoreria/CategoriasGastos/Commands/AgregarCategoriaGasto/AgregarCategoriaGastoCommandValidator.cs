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
            .MustAsync(
                async (nombre, cancellationToken) =>
                {
                    var empresaId = currentUser.EmpresaId;

                    return !await repository.ExistePorNombreAsync(nombre, empresaId);
                }
            )
            .WithMessage("Ya existe esa Categoria Gasto.");
    }
}
