// using Concesionaria.Application.Common.Interfaces;
// using MediatR;
// using Microsoft.EntityFrameworkCore;

// namespace Application.Features.CategoriasGastos.Commands.DesactivarCategoriaGasto;

// public class DesactivarCategoriaGastoCommandHandler
//     : IRequestHandler<DesactivarCategoriaGastoCommand, Unit>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly ICurrentUserService _currentUser;

//     public DesactivarCategoriaGastoCommandHandler(
//         IApplicationDbContext context,
//         ICurrentUserService currentUser
//     )
//     {
//         _context = context;
//         _currentUser = currentUser;
//     }

//     public async Task<Unit> Handle(
//         DesactivarCategoriaGastoCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         var empresaId = _currentUser.EmpresaId;
//         var res = request;
        
//         var obtenerCategoriaId = await _context.CategoriasGastos.IgnoreQueryFilters().FirstOrDefaultAsync(
//             cg => cg.Id == request.CategoriaGastoId && cg.EmpresaId == empresaId && !cg.Eliminado,
//             cancellationToken
//         );

//         if (obtenerCategoriaId == null)
//             throw new Exception("Categoría de gasto no encontrada.");

//         obtenerCategoriaId.Desactivar();

//         await _context.SaveChangesAsync(cancellationToken);

//         return Unit.Value;
//     }
// }