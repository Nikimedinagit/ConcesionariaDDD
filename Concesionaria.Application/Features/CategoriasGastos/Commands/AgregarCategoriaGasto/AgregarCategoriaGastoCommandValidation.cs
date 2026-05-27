using FluentValidation;
using Concesionaria.Domain.Interfaces.IRepositories;
using System.Threading.Tasks;

namespace Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;

public class AgregarCategoriaGastoCommandValidation
    : AbstractValidator<AgregarCategoriaGastoCommand>
{
    private readonly ICategoriaGastoRepository _repository;

    public AgregarCategoriaGastoCommandValidation(ICategoriaGastoRepository repository)
    {
        _repository = repository;

        RuleFor(cg => cg.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MustAsync(async (nombre, cancellationToken) => !await _repository.ExistePorNombreAsync(nombre))
            .WithMessage("Ya existe una categoría de gasto con ese nombre.");
    }

    private async Task<bool> NoExistirCategoria(string nombre, CancellationToken cancellationToken)
    {
        return !await _repository.ExistePorNombreAsync(nombre);
    }
}
