using Concesionaria.Domain.CategoriasGastos;
using Concesionaria.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CategoriasGastos.Commands.AgregarCategoriaGasto;

public class AgregarCategoriaGastoCommandHandler
    : IRequestHandler<AgregarCategoriaGastoCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public AgregarCategoriaGastoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        AgregarCategoriaGastoCommand request,
        CancellationToken cancellationToken
    )
    {
        var empresa = await _context.Empresas
            .FirstOrDefaultAsync(e => e.Id == request.EmpresaId, cancellationToken);

        if (empresa == null)
            throw new Exception("Empresa no encontrada");

        var categoriaGasto = CategoriaGasto.Crear(request.Nombre, empresa);

        await _context.CategoriasGastos.AddAsync(categoriaGasto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return categoriaGasto.Id;
    }
}
