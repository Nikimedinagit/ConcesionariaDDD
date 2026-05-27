using Concesionaria.Application.Common.Interfaces;
using MediatR;
namespace Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;

public class AgregarCategoriaGastoCommandHandler
    : IRequestHandler<AgregarCategoriaGastoCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AgregarCategoriaGastoCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
    AgregarCategoriaGastoCommand request,
    CancellationToken cancellationToken)
{
    var empresaId = _currentUser.EmpresaId;

    var categoriaGasto = CategoriaGasto.Crear(
        request.Nombre,
        empresaId
    );

    await _context.CategoriasGastos.AddAsync(categoriaGasto, cancellationToken);
    await _context.SaveChangesAsync(cancellationToken);

    return categoriaGasto.Id;
}
}