using MediatR;

namespace Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;

public record ObtenerCategoriasGastosActivasQuery : IRequest<List<CategoriasGastosDto>>;
