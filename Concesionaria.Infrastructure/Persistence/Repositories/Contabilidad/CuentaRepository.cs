using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CuentaRepository : ICuentaRepository
{
    private readonly ApplicationDbContext _context;

    public CuentaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //METODO PARA AGREGAR
    public async Task AddAsync(Cuenta cuenta)
    {
        await _context.Cuentas.AddAsync(cuenta);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync(Cuenta cuenta)
    {
        _context.Cuentas.Update(cuenta);
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Cuenta>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerCuentasActivas = _context.Cuentas.Where(c => c.EmpresaId == empresaId).AsQueryable();
        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerCuentasActivas = obtenerCuentasActivas.Where(c =>
                c.Codigo.Contains(filtro) ||
                c.Nombre.Contains(filtro));
        }
        return await obtenerCuentasActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Cuenta>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerCuentasInactivas = _context.Cuentas
            .IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerCuentasInactivas = obtenerCuentasInactivas.Where(c =>
                c.Codigo.Contains(filtro) ||
                c.Nombre.Contains(filtro));
        }
        return await obtenerCuentasInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA POR NOMBRE PARA AGREGAR
    public async Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId)
    {
        return await _context.Cuentas.AnyAsync(c =>
            c.EmpresaId == empresaId && c.Nombre.ToLower() == nombre.ToLower()
        );
    }

    // METODO PARA VALIDAR EXISTENCIA POR CODIGO PARA AGREGAR
    public async Task<bool> ExistePorCodigoAsync(string codigo, Guid empresaId)
    {
        return await _context.Cuentas.AnyAsync(c =>
            c.EmpresaId == empresaId && c.Codigo.ToLower() == codigo.ToLower()
        );
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorNombreExluyendoIdAsync(
     string nombre,
     Guid empresaId,
     Guid cuentaId)
    {
        return await _context.Cuentas.AnyAsync(c =>
            c.Nombre.ToLower() == nombre.ToLower()
            && c.EmpresaId == empresaId
            && c.Id != cuentaId
            && !c.Eliminado
        );
    }

}
