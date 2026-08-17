using Concesionaria.Application.Common.Interfaces;
using MediatR;

public class AgregarClienteCommandHandler
    : IRequestHandler<AgregarClienteCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarClienteCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser
    )
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        AgregarClienteCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresaId = _currentUser.EmpresaId;

        var cliente = Cliente.Crear(request.NombreCompleto, request.Dni,  request.Telefono, request.Email, request.Domicilio,  request.LocalidadId, empresaId);

        await _context.Clientes.AddAsync(cliente, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return cliente.Id;
    }
}
