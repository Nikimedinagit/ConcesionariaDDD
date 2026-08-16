using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MarcaVehiculoRepository : IMarcaVehiculoRepository
{
    private readonly ApplicationDbContext _context;

    public MarcaVehiculoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(MarcaVehiculo marcaVehiculo)
    {
        await _context.MarcasVehiculos.AddAsync(marcaVehiculo);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<MarcaVehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerMarcasActivas = _context.MarcasVehiculos
            .Where(mv => mv.EmpresaId == empresaId)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerMarcasActivas = obtenerMarcasActivas
                .Where(mv => mv.Nombre.Contains(filtro));
        }

        return await obtenerMarcasActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<MarcaVehiculo>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerMarcasInactivas = _context.MarcasVehiculos
            .IgnoreQueryFilters()
                .Where(mv => mv.EmpresaId == empresaId && mv.Eliminado)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerMarcasInactivas = obtenerMarcasInactivas
                .Where(mv => mv.Nombre.Contains(filtro));
        }

        return await obtenerMarcasInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId)
    {
        return await _context.MarcasVehiculos.AnyAsync(mv =>
            mv.EmpresaId == empresaId && mv.Nombre.ToLower() == nombre.ToLower()
        );
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorNombreExluyendoIdAsync(
     string nombre,
     Guid empresaId,
     Guid marcaVehiculoId)
    {
        return await _context.MarcasVehiculos.AnyAsync(mv =>
            mv.Nombre.ToLower() == nombre.ToLower()
            && mv.EmpresaId == empresaId
            && mv.Id != marcaVehiculoId
            && !mv.Eliminado
        );
    }

    // METODO PARA VALIDAR RELACION CON MODELOS
    public async Task<bool> TieneModelosActivosAsync(Guid empresaId, Guid marcaVehiculoId)
    {
        return await _context.ModelosVehiculos.AnyAsync(mv =>
            mv.EmpresaId == empresaId
            && mv.MarcaVehiculoId == marcaVehiculoId
            && !mv.Eliminado
        );
    }
}
