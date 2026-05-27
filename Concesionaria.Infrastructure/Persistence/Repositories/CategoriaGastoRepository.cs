using Concesionaria.Domain.CategoriasGastos;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public async Task AddAsync(CategoriaGasto categoriaGasto)
    {
        await _context.CategoriasGastos.AddAsync(categoriaGasto);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistePorNombreAsync(string nombre)
    {
        return await _context.CategoriasGastos.AnyAsync(cg =>
            cg.Nombre.ToLower() == nombre.ToLower()
        );
    }
}
