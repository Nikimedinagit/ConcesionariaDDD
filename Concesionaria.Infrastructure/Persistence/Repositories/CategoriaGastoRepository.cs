using Concesionaria.Domain.CategoriasGastos;
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

    public async Task<List<CategoriaGasto>> ObtenerActivasAsync()
    {
        return await _context.CategoriasGastos.Where(cg => !cg.Eliminado).ToListAsync();
    }

    public async Task<List<CategoriaGasto>> ObtenerInactivasAsync()
    {
        return await _context.CategoriasGastos.Where(cg => cg.Eliminado).ToListAsync();
    }
}
