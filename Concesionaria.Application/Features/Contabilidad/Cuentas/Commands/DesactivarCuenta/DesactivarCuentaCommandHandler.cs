using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Cuentas.Enums;
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

        var esCuentaBase =
            obtenerCuentaId.Nivel == 0 &&
            obtenerCuentaId.CuentaPadreId == null &&
            (
                obtenerCuentaId.Codigo == "1" && obtenerCuentaId.Nombre == "ACTIVO" && obtenerCuentaId.Tipo == TipoCuenta.ACTIVO ||
                obtenerCuentaId.Codigo == "2" && obtenerCuentaId.Nombre == "PASIVO" && obtenerCuentaId.Tipo == TipoCuenta.PASIVO ||
                obtenerCuentaId.Codigo == "3" && obtenerCuentaId.Nombre == "PATRIMONIO NETO" && obtenerCuentaId.Tipo == TipoCuenta.PATRIMONIO ||
                obtenerCuentaId.Codigo == "4" && obtenerCuentaId.Nombre == "INGRESO" && obtenerCuentaId.Tipo == TipoCuenta.INGRESO ||
                obtenerCuentaId.Codigo == "5" && obtenerCuentaId.Nombre == "EGRESO" && obtenerCuentaId.Tipo == TipoCuenta.EGRESO
            );

        if (esCuentaBase)
            throw new Exception("Las cuentas base no se pueden desactivar.");

        obtenerCuentaId.Desactivar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
