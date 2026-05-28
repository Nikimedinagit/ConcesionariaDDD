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

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<CategoriaGasto>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerCategoriasActivas = _context.CategoriasGastos
            .Where(cg => cg.EmpresaId == empresaId)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerCategoriasActivas = obtenerCategoriasActivas
                .Where(cg => cg.Nombre.Contains(filtro));
        }

        return await obtenerCategoriasActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<CategoriaGasto>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerCategoriasInactivas = _context.CategoriasGastos
            .IgnoreQueryFilters()
                .Where(cg => cg.EmpresaId == empresaId && cg.Eliminado)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerCategoriasInactivas = obtenerCategoriasInactivas
                .Where(cg => cg.Nombre.Contains(filtro));
        }

        return await obtenerCategoriasInactivas.ToListAsync();
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
     Guid categoriaGastoId)
    {
        return await _context.CategoriasGastos.AnyAsync(cg =>
            cg.Nombre.ToLower() == nombre.ToLower()
            && cg.EmpresaId == empresaId
            && cg.Id != categoriaGastoId
            && !cg.Eliminado
        );
    }
}
