using Concesionaria.Application;
using Concesionaria.Application.Interfaces;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.Infrastructure.Services;

public class LocalidadService : ILocalidadService
{
    private readonly ApplicationDbContext _context;

    public LocalidadService(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<ProvinciaDto>> GetAllProvinciasAsync()
    {
        return await _context.Provincias
            .Select(p => new ProvinciaDto(p.Id, p.Nombre))
            .ToListAsync();
    }

    public async Task<IEnumerable<LocalidadDto>> GetAllLocalidadesAsync()
    {
        return await _context.Localidades
            .Select(l => new LocalidadDto(l.Id, l.Nombre, l.CodigoPostal))
            .ToListAsync();
    }
}