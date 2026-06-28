using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Cuentas.Commands.ActualizarCuenta;

public class ActualizarCuentaCommandHandler
    : IRequestHandler<ActualizarCuentaCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarCuentaCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarCuentaCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerCuentaId = await _context.Cuentas.FirstOrDefaultAsync(
            c => c.Id == request.CuentaId && c.EmpresaId == empresaId && !c.Eliminado,
            cancellationToken
        );

        if (obtenerCuentaId == null)
            throw new Exception("Cuenta no encontrada.");

        //ACTUALIZACIONES PARCIALES

        if (request.Nombre is not null)
            obtenerCuentaId.ActualizarNombre(request.Nombre);

        // if (request.Codigo is not null)
        //     obtenerCuentaId.ActualizarCodigo(request.Codigo);

        // if (request.Tipo.HasValue)
        //     obtenerCuentaId.ActualizarTipo(request.Tipo.Value);

        // if (request.Nivel.HasValue)
        //     obtenerCuentaId.ActualizarNivel(request.Nivel.Value);

        //AUDITORIA
        // obtenerCuentaId.UpdatedAt = DateTime.UtcNow;
        // obtenerCuentaId.UpdatedBy = _currentUser.UserId;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
