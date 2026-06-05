using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SucursalRepository : ISucursalRepository
{
    private readonly ApplicationDbContext _context;

    public SucursalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sucursal>> ObtenerActivasAsync()
    {
        return await _context.Sucursales.Where(s => !s.Eliminado).ToListAsync();
    }

    public async Task<List<Sucursal>> ObtenerInactivasAsync()
    {
        return await _context.Sucursales.Where(s => s.Eliminado).ToListAsync();
    }
}
