// using Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosActivas;
// using Concesionaria.Application.Common.Interfaces;
// using Concesionaria.Domain.Interfaces.IRepositories;
// using MediatR;

// namespace Application.Features.CategoriasGastos.Queries.ObtenerCategoriasGastosInactivas;

// public class ObtenerCategoriasGastosInactivasQueryHandler
//     : IRequestHandler<ObtenerCategoriasGastosInactivasQuery, List<CategoriasGastosDto>>
// {
//     private readonly ICategoriaGastoRepository _repository;
//     private readonly ICurrentUserService _currentUserService;

//     public ObtenerCategoriasGastosInactivasQueryHandler(ICategoriaGastoRepository repository, ICurrentUserService currentUserService)
//     {
//         _repository = repository;
//         _currentUserService = currentUserService;
//     }

//     public async Task<List<CategoriasGastosDto>> Handle(
//         ObtenerCategoriasGastosInactivasQuery request,
//         CancellationToken cancellationToken)
//     {
//         var empresaId = _currentUserService.EmpresaId;
//         var categorias = await _repository.ObtenerInactivasAsync(empresaId, request.Filtro);

//         return categorias
//             .OrderBy(cg => cg.Nombre)
//             .Select(cg => new CategoriasGastosDto
//             {
//                 CategoriaGastoId = cg.Id,
//                 Nombre = cg.Nombre
//             })
//             .ToList();
//     }
// }

