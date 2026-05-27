using MediatR;

namespace Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;

public record AgregarCategoriaGastoCommand : IRequest<Guid>
{
    public Guid EmpresaId { get; init; }
    public string Nombre { get; init; } = string.Empty;
}
