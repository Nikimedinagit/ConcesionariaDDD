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

    // METODO PARA AGREGAR
    public async Task AddAsync(CategoriaGasto categoriaGasto)
    {
        await _context.CategoriasGastos.AddAsync(categoriaGasto);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS
    public async Task<List<CategoriaGasto>> ObtenerActivasAsync()
    {
        return await _context.CategoriasGastos.ToListAsync();
    }

    // METODO PARA OBTENER INCATIVAS
    public async Task<List<CategoriaGasto>> ObtenerInactivasAsync()
    {
        return await _context
            .CategoriasGastos.IgnoreQueryFilters()
            .Where(x => x.Eliminado)
            .ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId)
    {
        return await _context.CategoriasGastos.AnyAsync(cg =>
            cg.EmpresaId == empresaId && cg.Nombre.ToLower() == nombre.ToLower()
        );
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorNombreExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid CategoriaGastoId
    )
    {
        return await _context.CategoriasGastos.AnyAsync(cg =>
            cg.EmpresaId == empresaId
            && cg.Nombre.ToLower() == nombre.ToLower()
            && cg.Id != CategoriaGastoId
        );
    }
}
