// using Concesionaria.Application.Common.Interfaces;
// using MediatR;
// using Microsoft.EntityFrameworkCore;

// namespace Application.Features.CategoriasGastos.Commands.ActualizarCategoriaGasto;

// public class ActualizarCategoriaGastoCommandHandler
//     : IRequestHandler<ActualizarCategoriaGastoCommand, Unit>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly ICurrentUserService _currentUser;

//     public ActualizarCategoriaGastoCommandHandler(
//         IApplicationDbContext context,
//         ICurrentUserService currentUser
//     )
//     {
//         _context = context;
//         _currentUser = currentUser;
//     }

//     public async Task<Unit> Handle(
//         ActualizarCategoriaGastoCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         var empresaId = _currentUser.EmpresaId;

//         var obtenerCategoriaId = await _context.CategoriasGastos.FirstOrDefaultAsync(
//             cg => cg.Id == request.CategoriaGastoId && cg.EmpresaId == empresaId && !cg.Eliminado,
//             cancellationToken
//         );

//         if (obtenerCategoriaId == null)
//             throw new Exception("Categoría de gasto no encontrada.");

//         obtenerCategoriaId.ActualizarCategoriaGasto(request.Nombre);

//         await _context.SaveChangesAsync(cancellationToken);
//         return Unit.Value;
//     }
// }
