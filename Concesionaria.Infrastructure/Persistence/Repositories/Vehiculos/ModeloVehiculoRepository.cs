using Concesionaria.Domain.Common.Enums;
using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ModeloVehiculoRepository : IModeloVehiculoRepository
{
    private readonly ApplicationDbContext _context;

    public ModeloVehiculoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(ModeloVehiculo modeloVehiculo)
    {
        await _context.ModelosVehiculos.AddAsync(modeloVehiculo);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<ModeloVehiculo>> ObtenerActivasAsync(
        Guid empresaId,
        string filtro = null,
        Guid? marcaVehiculoId = null,
        Guid? tipoVehiculoId = null
    )
    {
        var obtenerModelosActivas = _context
            .ModelosVehiculos.Where(mv => mv.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerModelosActivas = obtenerModelosActivas.Where(mv => mv.Nombre.Contains(filtro));
        }

        if (marcaVehiculoId.HasValue)
            obtenerModelosActivas = obtenerModelosActivas.Where(mv => mv.MarcaVehiculoId == marcaVehiculoId.Value);

        if (tipoVehiculoId.HasValue)
            obtenerModelosActivas = obtenerModelosActivas.Where(mv => mv.TipoVehiculoId == tipoVehiculoId.Value);

        return await obtenerModelosActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<ModeloVehiculo>> ObtenerInactivasAsync(
        Guid empresaId,
        string filtro = null,
        Guid? marcaVehiculoId = null,
        Guid? tipoVehiculoId = null
    )
    {
        var obtenerModelosInactivas = _context
            .ModelosVehiculos.IgnoreQueryFilters()
            .Where(mv => mv.EmpresaId == empresaId && mv.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerModelosInactivas = obtenerModelosInactivas.Where(mv =>
                mv.Nombre.Contains(filtro)
            );
        }

        if (marcaVehiculoId.HasValue)
            obtenerModelosInactivas = obtenerModelosInactivas.Where(mv => mv.MarcaVehiculoId == marcaVehiculoId.Value);

        if (tipoVehiculoId.HasValue)
            obtenerModelosInactivas = obtenerModelosInactivas.Where(mv => mv.TipoVehiculoId == tipoVehiculoId.Value);

        return await obtenerModelosInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<EstadoExistencia> ExistePorNombreAsync(
        string nombre,
        Guid empresaId,
        Guid tipoVehiculoId,
        Guid marcaVehiculoId
    )
    {
        var normalized = nombre.Trim();

        bool? entidad = await _context
            .ModelosVehiculos.IgnoreQueryFilters()
            .Where(m =>
                m.EmpresaId == empresaId
                && m.Nombre == normalized
                && m.TipoVehiculoId == tipoVehiculoId
                && m.MarcaVehiculoId == marcaVehiculoId
            )
            .Select(m => (bool?)m.Eliminado)
            .FirstOrDefaultAsync();

        if (entidad == null)
            return EstadoExistencia.NoExiste;
        return entidad.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<EstadoExistencia> ExistePorNombreTipoMarcaExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid modeloVehiculoId,
        Guid tipoVehiculoId,
        Guid marcaVehiculoId
    )
    {
        bool? entidad = await _context
            .ModelosVehiculos.IgnoreQueryFilters()
            .Where(mv =>
                mv.Nombre.ToLower() == nombre.ToLower()
                && mv.EmpresaId == empresaId
                && mv.Id != modeloVehiculoId
                && mv.TipoVehiculoId == tipoVehiculoId
                && mv.MarcaVehiculoId == marcaVehiculoId
            )
            .Select(mv => (bool?)mv.Eliminado)
            .FirstOrDefaultAsync();

        if (entidad == null)
            return EstadoExistencia.NoExiste;
        return entidad.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }
}
