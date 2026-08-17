using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.ActivarCliente;

public class ActivarClienteCommandHandler
    : IRequestHandler<ActivarClienteCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActivarClienteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActivarClienteCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;
        
        var obtenerModeloId = await _context.Clientes.IgnoreQueryFilters().FirstOrDefaultAsync(
            m => m.Id == request.ClienteId && m.EmpresaId == empresaId && m.Eliminado,
            cancellationToken
        );

        if (obtenerModeloId == null)
            throw new Exception("Cliente no encontrado.");

        obtenerModeloId.Activar();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
