using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Personas.Commands.ActualizarCliente;
public class ActualizarClienteCommandHandler
    : IRequestHandler<ActualizarClienteCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ActualizarClienteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(
        ActualizarClienteCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var obtenerClienteId = await _context.Clientes.FirstOrDefaultAsync(
            m => m.Id == request.ClienteId && m.EmpresaId == empresaId && !m.Eliminado,
            cancellationToken
        );

        if (obtenerClienteId == null)
            throw new Exception("Cliente no encontrado.");

        obtenerClienteId.ActualizarCliente(request.NombreCompleto, request.Dni,  request.Telefono, request.Email, request.Domicilio,  request.LocalidadId);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
