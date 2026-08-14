using MediatR;

namespace Application.Features.CategoriasGastos.Commands.ActivarCategoriaGasto;

public record ActivarCategoriaGastoCommand : IRequest<Unit>{
    public Guid CategoriaGastoId { get; init; }
    public bool Eliminado { get; init; }
}
