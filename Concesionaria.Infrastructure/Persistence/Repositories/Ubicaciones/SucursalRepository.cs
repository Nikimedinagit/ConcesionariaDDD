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

    //TAREA PARA AGREGAR
    public async Task AddAsync(Sucursal sucursal)
    {
        await _context.Sucursales.AddAsync(sucursal);
    }

    //METODO PARA OBTENER LAS SUCURSALES ACTIVAS SEGUN FILTRO
    public async Task<List<Sucursal>> ObtenerActivasAsync(Guid empresaId, string filtro)
    {
        var obtenerSucurasActivas = _context
            .Sucursales.Where(s => s.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerSucurasActivas = obtenerSucurasActivas.Where(s =>
                s.Nombre.Contains(filtro)
                || s.Direccion.Contains(filtro)
                || s.LocalidadId.ToString().Contains(filtro)
            );
        }

        return await obtenerSucurasActivas.ToListAsync();
    }

    //METODO PARA OBTENER LAS SUCURSALES INACTIVAS SEGUN FILTRO
    public async Task<List<Sucursal>> ObtenerInactivasAsync(Guid empresaId, string filtro)
    {
        var obtenerSucursalesInactivas = _context
            .Sucursales.IgnoreQueryFilters()
            .Where(s => s.EmpresaId == empresaId && s.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filtro))
        {
            obtenerSucursalesInactivas = obtenerSucursalesInactivas.Where(s =>
                s.Nombre.Contains(filtro)
                || s.Direccion.Contains(filtro)
                || s.LocalidadId.ToString().Contains(filtro)
            );
        }

        return await obtenerSucursalesInactivas.ToListAsync();
    }

    public async Task<NombreSucursalEstado> ExistePorNombreLocalidadAsync(
        string nombre,
        Guid empresaId,
        Guid localidadId
    )
    {
        var eliminado = await _context.Sucursales
            .IgnoreQueryFilters()
            .Where(s =>
                s.EmpresaId == empresaId
                && s.LocalidadId == localidadId
                && s.Nombre.ToLower() == nombre.Trim().ToLower()
            )
            .Select(s => (bool?)s.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => NombreSucursalEstado.NoExiste,
            true => NombreSucursalEstado.Desactivado,
            _ => NombreSucursalEstado.Activo,
        };
    }

    public async Task<NombreSucursalEstado> ExistePorNombreLocalidadAsync(
        string nombre,
        Guid empresaId,
        Guid localidadId,
        Guid sucursalId
    )
    {
        var eliminado = await _context.Sucursales
            .IgnoreQueryFilters()
            .Where(s =>
                s.EmpresaId == empresaId
                && s.LocalidadId == localidadId
                && s.Id != sucursalId
                && s.Nombre.ToLower() == nombre.Trim().ToLower()
            )
            .Select(s => (bool?)s.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => NombreSucursalEstado.NoExiste,
            true => NombreSucursalEstado.Desactivado,
            _ => NombreSucursalEstado.Activo,
        };
    }

    public async Task<bool> LocalidadExisteAsync(Guid localidadId)
    {
        return await _context.Localidades.AnyAsync(s => s.Id == localidadId);
    }

    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }
}
