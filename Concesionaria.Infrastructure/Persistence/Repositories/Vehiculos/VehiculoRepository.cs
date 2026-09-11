using Concesionaria.Domain.Cuentas.Enums;
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

    // METODO PARA OBTENER DISONIBLES SEGUN FILTRO
    public async Task<List<Vehiculo>> ObtenerDisponiblesAsync(
        Guid empresaId,
        Guid sucursalId,
        string filtro = null)
    {
        var vehiculosActivosQuery = _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(vehiculo =>
                vehiculo.EmpresaId == empresaId &&
                vehiculo.SucursalId == sucursalId &&
                !vehiculo.Eliminado &&
                vehiculo.Estado == EstadoVehiculo.DISPONIBLE)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            vehiculosActivosQuery = vehiculosActivosQuery.Where(vehiculo =>
                vehiculo.Version.Contains(filtro)
            );
        }

        return await vehiculosActivosQuery.ToListAsync();
    }

    // METODO PARA OBTENER RESERVADOS SEGUN FILTRO
    public async Task<List<Vehiculo>> ObtenerReservadosAsync(
        Guid empresaId,
        Guid sucursalId,
        string filtro = null)
    {
        var vehiculosActivosQuery = _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(vehiculo =>
                vehiculo.EmpresaId == empresaId &&
                vehiculo.SucursalId == sucursalId &&
                !vehiculo.Eliminado &&
                vehiculo.Estado == EstadoVehiculo.RESERVADO)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            vehiculosActivosQuery = vehiculosActivosQuery.Where(vehiculo =>
                vehiculo.Version.Contains(filtro)
            );
        }

        return await vehiculosActivosQuery.ToListAsync();
    }

    // METODO PARA OBTENER EN REPARACION SEGUN FILTRO
    public async Task<List<Vehiculo>> ObtenerEnReparacionAsync(
        Guid empresaId,
        Guid sucursalId,
        string filtro = null)
    {
        var vehiculosActivosQuery = _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(vehiculo =>
                vehiculo.EmpresaId == empresaId &&
                vehiculo.SucursalId == sucursalId &&
                !vehiculo.Eliminado &&
                vehiculo.Estado == EstadoVehiculo.EN_SERVICIO)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            vehiculosActivosQuery = vehiculosActivosQuery.Where(vehiculo =>
                vehiculo.Version.Contains(filtro)
            );
        }

        return await vehiculosActivosQuery.ToListAsync();
    }

    // METODO PARA OBTENER VENDIDOS SEGUN FILTRO
    public async Task<List<Vehiculo>> ObtenerVendidosAsync(
        Guid empresaId,
        Guid sucursalId,
        string filtro = null)
    {
        var vehiculosVendidosQuery = _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(vehiculo =>
                vehiculo.EmpresaId == empresaId &&
                vehiculo.SucursalId == sucursalId &&
                !vehiculo.Eliminado &&
                vehiculo.Estado == EstadoVehiculo.VENDIDO)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            vehiculosVendidosQuery = vehiculosVendidosQuery.Where(vehiculo =>
                vehiculo.Version.Contains(filtro)
            );
        }

        return await vehiculosVendidosQuery.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<bool> ExistePorPatenteAsync(string patente, Guid empresaId)
    {
        var normalized = patente.ToUpper().Trim();

        bool entidad = await _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(m => m.EmpresaId == empresaId && m.Patente == normalized)
            .AnyAsync();

        return entidad;
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorPatenteExluyendoIdAsync(
        string patente,
        Guid empresaId,
        Guid vehiculoId
    )
    {
        var normalized = patente.ToUpper().Trim();

        bool entidad = await _context
            .Vehiculos.IgnoreQueryFilters()
            .Where(m => m.EmpresaId == empresaId && m.Patente == normalized && m.Id != vehiculoId)
            .AnyAsync();

        return entidad;
    }

    // METODO PARA OBTENER POR MODELO ID
    public async Task<bool> ObtenerPorModeloIdAsync(Guid modeloId, Guid empresaId)
    {
        // AnyAsync devuelve true si existe al menos un registro
        bool existe = await _context
            .Vehiculos.IgnoreQueryFilters()
            .AnyAsync(v => v.ModeloId == modeloId && v.EmpresaId == empresaId);

        return existe; // devolvés directamente el resultado
    }
}
