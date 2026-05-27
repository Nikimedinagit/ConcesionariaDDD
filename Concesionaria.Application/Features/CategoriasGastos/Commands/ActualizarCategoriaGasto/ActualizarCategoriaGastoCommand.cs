using MediatR;

namespace Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;

public record ActualizarCategoriaGastoCommand : IRequest
{
    public Guid CategoriaGastoId { get; init; }
    public string Nombre { get; init; } = string.Empty;
}
