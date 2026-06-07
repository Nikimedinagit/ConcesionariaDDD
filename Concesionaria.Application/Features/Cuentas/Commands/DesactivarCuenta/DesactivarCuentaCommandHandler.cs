using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Cuentas.Commands.DesactivarCuenta;

public class DesactivarCuentaCommandHandler
    : IRequestHandler<DesactivarCuentaCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DesactivarCuentaCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        DesactivarCuentaCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        var res = request;
        
        var obtenerCuentaId = await _context.Cuentas.IgnoreQueryFilters().FirstOrDefaultAsync(
            c => c.Id == request.CuentaId && c.EmpresaId == empresaId && !c.Eliminado,
            cancellationToken
        );

        if (obtenerCuentaId == null)
            throw new Exception("Cuenta no encontrada.");

        obtenerCuentaId.Desactivar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}