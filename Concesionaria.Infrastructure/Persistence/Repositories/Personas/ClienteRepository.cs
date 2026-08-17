using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ApplicationDbContext _context;

    public ClienteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Cliente>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerClientesActivos = _context.Clientes
            .Where(c => c.EmpresaId == empresaId)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerClientesActivos = obtenerClientesActivos
                .Where(c => c.NombreCompleto.Contains(filtro));
        }

        return await obtenerClientesActivos.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Cliente>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {

        var obtenerClientesInactivos = _context.Clientes
            .IgnoreQueryFilters()
                .Where(c => c.EmpresaId == empresaId && c.Eliminado)
                    .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            obtenerClientesInactivos = obtenerClientesInactivos
                .Where(c => c.NombreCompleto.Contains(filtro));
        }

        return await obtenerClientesInactivos.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<bool> ExistePorDniAsync(string dni, Guid empresaId)
    {
        return await _context.Clientes.AnyAsync(c =>
            c.EmpresaId == empresaId && c.Dni == dni
        );
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<bool> ExistePorDniExcluyendoIdAsync(
     string dni,
     Guid empresaId,
     Guid clienteId)
    {
        return await _context.Clientes.AnyAsync(c =>
            c.Dni == dni
            && c.EmpresaId == empresaId
            && c.Id != clienteId
            && !c.Eliminado
        );
    }

    
}
