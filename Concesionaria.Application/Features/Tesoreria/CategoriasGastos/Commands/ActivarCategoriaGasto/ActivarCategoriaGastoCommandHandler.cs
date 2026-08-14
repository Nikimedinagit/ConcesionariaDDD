using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CategoriasGastos.Commands.ActivarCategoriaGasto;

public class ActivarCategoriaGastoCommandHandler
    : IRequestHandler<ActivarCategoriaGastoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarCategoriaGastoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarCategoriaGastoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var res = request;
        
        var obtenerCategoriaId = await _context.CategoriasGastos.IgnoreQueryFilters().FirstOrDefaultAsync(
            cg => cg.Id == request.CategoriaGastoId && cg.EmpresaId == empresaId && cg.Eliminado,
            cancellationToken
        );

        if (obtenerCategoriaId == null)
            throw new Exception("Categoría de gasto no encontrada.");

        obtenerCategoriaId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
