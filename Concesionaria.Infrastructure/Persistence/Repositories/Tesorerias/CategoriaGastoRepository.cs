using Concesionaria.Domain.Common.Enums;
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
    public async Task<EstadoExistencia> ExistePorNombreAsync(string nombre, Guid empresaId)
    {
        var eliminado = await _context.CategoriasGastos
            .IgnoreQueryFilters()
            .Where(cg => cg.EmpresaId == empresaId && cg.Nombre.ToLower() == nombre.Trim().ToLower())
            .Select(cg => (bool?)cg.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => EstadoExistencia.NoExiste,
            true => EstadoExistencia.Desactivado,
            _ => EstadoExistencia.Activo,
        };
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<EstadoExistencia> ExistePorNombreExluyendoIdAsync(
     string nombre,
     Guid empresaId,
     Guid categoriaGastoId)
    {
        var eliminado = await _context.CategoriasGastos
            .IgnoreQueryFilters()
            .Where(cg => cg.Nombre.ToLower() == nombre.Trim().ToLower() && cg.EmpresaId == empresaId && cg.Id != categoriaGastoId)
            .Select(cg => (bool?)cg.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => EstadoExistencia.NoExiste,
            true => EstadoExistencia.Desactivado,
            _ => EstadoExistencia.Activo,
        };
    }

    
}
