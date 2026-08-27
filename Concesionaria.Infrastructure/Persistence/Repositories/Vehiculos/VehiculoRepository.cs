using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly ApplicationDbContext _context;

    public VehiculoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(Vehiculo vehiculo)
    {
        await _context.Vehiculos.AddAsync(vehiculo);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Vehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerVehiculosActivos = _context
            .Vehiculos.Where(v => v.EmpresaId == empresaId && v.Estado != EstadoVehiculo.Vendido)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerVehiculosActivos = obtenerVehiculosActivos.Where(v =>
                v.Version.Contains(filtro)
            );
        }

        // if (sucursalId.HasValue)
        //     obtenerVehiculosActivos = obtenerVehiculosActivos.Where(v => v.SucursalId == sucursalId.Value);

        return await obtenerVehiculosActivos.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<bool> ExistePorPatenteAsync(string patente, Guid empresaId)
    {
        var normalized = patente.Trim();

        bool? entidad = await _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(m => m.EmpresaId == empresaId && m.Patente == normalized)
            .AnyAsync();

        if (entidad == null)
            return false;
        return entidad.Value ? false : true;
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorPatenteExluyendoIdAsync(
        string patente,
        Guid empresaId,
        Guid vehiculoId
    )
    {
        var normalized = patente.Trim();

        bool? entidad = await _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(m => m.EmpresaId == empresaId && m.Patente == normalized && m.Id != vehiculoId)
            .AnyAsync();

        if (entidad == null)
            return false;
        return entidad.Value ? false : true;
    }
}
