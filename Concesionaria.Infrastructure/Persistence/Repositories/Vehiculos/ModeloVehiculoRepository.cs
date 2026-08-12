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
        string filtro = null
    )
    {
        var obtenerModelosActivas = _context
            .ModelosVehiculos.Where(mv => mv.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerModelosActivas = obtenerModelosActivas.Where(mv => mv.Nombre.Contains(filtro));
        }

        return await obtenerModelosActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<ModeloVehiculo>> ObtenerInactivasAsync(
        Guid empresaId,
        string filtro = null
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

        return await obtenerModelosInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId)
    {
        return await _context.ModelosVehiculos.AnyAsync(mv =>
            mv.EmpresaId == empresaId && mv.Nombre.ToLower() == nombre.ToLower()
        );
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorNombreExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid modeloVehiculoId
    )
    {
        return await _context.ModelosVehiculos.AnyAsync(mv =>
            mv.Nombre.ToLower() == nombre.ToLower()
            && mv.EmpresaId == empresaId
            && mv.Id != modeloVehiculoId
            && !mv.Eliminado
        );
    }
}
