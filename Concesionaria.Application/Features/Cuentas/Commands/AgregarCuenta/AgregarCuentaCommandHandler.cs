using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Cuentas;
using MediatR;

namespace Application.Features.Cuentas.Commands.AgregarCuenta;

public class AgregarCuentaCommandHandler
    : IRequestHandler<AgregarCuentaCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarCuentaCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarCuentaCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var cuenta = Cuenta.Crear(empresaId, request.Nombre, request.Codigo, request.Tipo, request.Nivel);

        await _context.Cuentas.AddAsync(cuenta, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return cuenta.Id;
    }
}
