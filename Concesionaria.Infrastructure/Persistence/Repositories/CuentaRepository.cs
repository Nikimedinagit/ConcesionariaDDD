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

    public async Task<List<Cuenta>> ObtenerActivasAsync()
    {
        return await _context.Cuentas.Where(c => !c.Eliminado).ToListAsync();
    }

    public async Task<List<Cuenta>> ObtenerInactivasAsync()
    {
        return await _context.Cuentas.Where(c => c.Eliminado).ToListAsync();
    }
}
