using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class TipoVehiculoRepository : ITipoVehiculoRepository
{
    private readonly ApplicationDbContext _context;

    public TipoVehiculoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(TipoVehiculo tipoVehiculo)
    {
        await _context.TiposVehiculos.AddAsync(tipoVehiculo);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<TipoVehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerTiposActivas = _context.TiposVehiculos
            .Where(tv => tv.EmpresaId == empresaId)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerTiposActivas = obtenerTiposActivas
                .Where(tv => tv.Nombre.Contains(filtro));
        }

        return await obtenerTiposActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<TipoVehiculo>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerTiposInactivas = _context.TiposVehiculos
            .IgnoreQueryFilters()
                .Where(tv => tv.EmpresaId == empresaId && tv.Eliminado)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerTiposInactivas = obtenerTiposInactivas
                .Where(tv => tv.Nombre.Contains(filtro));
        }

        return await obtenerTiposInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<NombreEntidadVehiculoEstado> ExistePorNombreAsync(string nombre, Guid empresaId)
    {
        var eliminado = await _context.TiposVehiculos
            .IgnoreQueryFilters()
            .Where(tv => tv.EmpresaId == empresaId && tv.Nombre.ToLower() == nombre.Trim().ToLower())
            .Select(tv => (bool?)tv.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => NombreEntidadVehiculoEstado.NoExiste,
            true => NombreEntidadVehiculoEstado.Desactivado,
            _ => NombreEntidadVehiculoEstado.Activo,
        };
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<NombreEntidadVehiculoEstado> ExistePorNombreExluyendoIdAsync(
     string nombre,
     Guid empresaId,
     Guid tipoVehiculoId)
    {
        var eliminado = await _context.TiposVehiculos
            .IgnoreQueryFilters()
            .Where(tv => tv.Nombre.ToLower() == nombre.Trim().ToLower() && tv.EmpresaId == empresaId && tv.Id != tipoVehiculoId)
            .Select(tv => (bool?)tv.Eliminado)
            .FirstOrDefaultAsync();

        return eliminado switch
        {
            null => NombreEntidadVehiculoEstado.NoExiste,
            true => NombreEntidadVehiculoEstado.Desactivado,
            _ => NombreEntidadVehiculoEstado.Activo,
        };
    }

      // METODO PARA VALIDAR RELACION CON MODELOS
    public async Task<bool> TieneModelosActivosAsync(Guid empresaId, Guid tipoVehiculoId)
    {
        return await _context.ModelosVehiculos.AnyAsync(mv =>
            mv.EmpresaId == empresaId
            && mv.TipoVehiculoId == tipoVehiculoId
            && !mv.Eliminado
        );
    }
}
