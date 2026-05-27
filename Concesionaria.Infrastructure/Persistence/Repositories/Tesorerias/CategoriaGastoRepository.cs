using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoriaGastoRepository : ICategoriaGastoRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriaGastoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CategoriaGasto categoriaGasto)
    {
        await _context.CategoriasGastos.AddAsync(categoriaGasto);
    }

    public async Task<List<CategoriaGasto>> ObtenerActivasAsync()
    {
        return await _context.CategoriasGastos.ToListAsync();
    }

    public async Task<List<CategoriaGasto>> ObtenerInactivasAsync()
    {
        return await _context.CategoriasGastos
            .IgnoreQueryFilters()
            .Where(x => x.Eliminado)
            .ToListAsync();
    }

    public async Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId)
{
    return await _context.CategoriasGastos
        .AnyAsync(cg =>
            cg.EmpresaId == empresaId &&
            cg.Nombre.ToLower() == nombre.ToLower());
}
}
