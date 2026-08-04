// using Concesionaria.Application.Common.Interfaces;
// using MediatR;
// using Microsoft.EntityFrameworkCore;

// namespace Application.Features.Vehiculos.Commands.DesactivarMarcaVehiculo;

// public class DesactivarMarcaVehiculoCommandHandler
//     : IRequestHandler<DesactivarMarcaVehiculoCommand, Unit>
// {
//     private readonly IApplicationDbContext _context;
//     private readonly ICurrentUserService _currentUser;

//     public DesactivarMarcaVehiculoCommandHandler(
//         IApplicationDbContext context,
//         ICurrentUserService currentUser
//     )
//     {
//         _context = context;
//         _currentUser = currentUser;
//     }

//     public async Task<Unit> Handle(
//         DesactivarMarcaVehiculoCommand request,
//         CancellationToken cancellationToken
//     )
//     {
//         var empresaId = _currentUser.EmpresaId;
//         var res = request;
        
//         var obtenerMarcaId = await _context.MarcasVehiculos.IgnoreQueryFilters().FirstOrDefaultAsync(
//             mv => mv.Id == request.MarcaVehiculoId && mv.EmpresaId == empresaId && !mv.Eliminado,
//             cancellationToken
//         );

//         if (obtenerMarcaId == null)
//             throw new Exception("Marca de vehículo no encontrada.");

//         obtenerMarcaId.Desactivar();

//         await _context.SaveChangesAsync(cancellationToken);

//         return Unit.Value;
//     }
// }