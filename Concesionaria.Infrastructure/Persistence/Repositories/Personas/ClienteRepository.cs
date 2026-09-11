using Concesionaria.Domain.Common.Enums;
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
        var obtenerClientesActivos = _context
            .Clientes.Where(c => c.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerClientesActivos = obtenerClientesActivos.Where(c =>
                c.NombreCompleto.ToLower().Contains(filtroNormalizado)
                || c.Dni.ToLower().Contains(filtroNormalizado)
                || c.Telefono.ToLower().Contains(filtroNormalizado)
                || c.Email.ToLower().Contains(filtroNormalizado)
                || c.Domicilio.ToLower().Contains(filtroNormalizado)
                || c.Localidad.Nombre.ToLower().Contains(filtroNormalizado)
            );
        }

        return await obtenerClientesActivos.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Cliente>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerClientesInactivos = _context
            .Clientes.IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerClientesInactivos = obtenerClientesInactivos.Where(c =>
                c.NombreCompleto.ToLower().Contains(filtroNormalizado)
            || c.Dni.ToLower().Contains(filtroNormalizado)
            || c.Telefono.ToLower().Contains(filtroNormalizado)
            || c.Email.ToLower().Contains(filtroNormalizado)
            || c.Domicilio.ToLower().Contains(filtroNormalizado)
            || c.Localidad.Nombre.ToLower().Contains(filtroNormalizado)
            );
        }

        return await obtenerClientesInactivos.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<EstadoExistencia> ExistePorDniAsync(string dni, Guid empresaId)
    {
        bool? estado = await _context.Clientes.IgnoreQueryFilters()
       .Where(c => c.EmpresaId == empresaId && c.Dni == dni)
       .Select(c => (bool?)c.Eliminado)
       .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    public async Task<EstadoExistencia> ExistePorEmailAsync(string email, Guid empresaId)
    {
        bool? estado = await _context.Clientes.IgnoreQueryFilters()
        .Where(c => c.EmpresaId == empresaId && c.Email.ToLower() == email.ToLower().Trim())
        .Select(c => (bool?)c.Eliminado)
        .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<EstadoExistencia> ExistePorDniExcluyendoIdAsync(
        string dni,
        Guid empresaId,
        Guid clienteId
    )
    {
        bool? estado = await _context.Clientes.IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Dni == dni && c.Id != clienteId)
            .Select(c => (bool?)c.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }

    public async Task<EstadoExistencia> ExistePorEmailExcluyendoIdAsync(
        string email,
        Guid empresaId,
        Guid clienteId
    )
    {
        bool? estado = await _context.Clientes.IgnoreQueryFilters()
            .Where(c => c.EmpresaId == empresaId && c.Email.ToLower() == email.ToLower().Trim() && c.Id != clienteId)
            .Select(c => (bool?)c.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return EstadoExistencia.NoExiste;

        return estado.Value ? EstadoExistencia.Desactivado : EstadoExistencia.Activo;
    }
}
