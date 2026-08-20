using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly ApplicationDbContext _context;

    public ProveedorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Proveedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerProveedoresActivas = _context
            .Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerProveedoresActivas = obtenerProveedoresActivas.Where(p =>
                p.Nombre.Contains(filtro)
            );
        }

        return await obtenerProveedoresActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Proveedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerProveedoresInactivas = _context
            .Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId && p.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerProveedoresInactivas = obtenerProveedoresInactivas.Where(p =>
                p.Nombre.Contains(filtroNormalizado)
            );
        }

        return await obtenerProveedoresInactivas.ToListAsync();
    }
}
