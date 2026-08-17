using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.DesactivarCliente;

public class DesactivarClienteCommandHandler
    : IRequestHandler<DesactivarClienteCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DesactivarClienteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        DesactivarClienteCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        
        var obtenerClienteId = await _context.Clientes.IgnoreQueryFilters().FirstOrDefaultAsync(
            mv => mv.Id == request.ClienteId && mv.EmpresaId == empresaId && !mv.Eliminado,
            cancellationToken
        );

        if (obtenerClienteId == null)
            throw new Exception("Cliente no encontrado.");

        obtenerClienteId.Desactivar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}