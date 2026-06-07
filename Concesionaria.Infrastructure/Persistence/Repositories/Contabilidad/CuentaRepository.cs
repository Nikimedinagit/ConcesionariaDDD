using Concesionaria.Domain.Cuentas;
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

    public async Task<List<Cuenta>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerCuentasInactivas = _context.Cuentas.Where(c => c.EmpresaId == empresaId && c.Eliminado).AsQueryable();
        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerCuentasInactivas = obtenerCuentasInactivas.Where(c =>
                c.Codigo.Contains(filtro) ||
                c.Nombre.Contains(filtro));
        }
        return await obtenerCuentasInactivas.ToListAsync();
    }
}
