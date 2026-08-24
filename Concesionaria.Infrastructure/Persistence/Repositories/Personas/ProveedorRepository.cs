using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProveedorRepository : IProveedorRepository
{
    private readonly ApplicationDbContext _context;

    public ProveedorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // METODO PARA AGREGAR
    public async Task AddAsync(Proveedor proveedor)
    {
        await _context.Proveedores.AddAsync(proveedor);
    }

    //METODO PARA ACTUALIZAR
    public async Task UpdateAsync()
    {
        await _context.SaveChangesAsync();
    }

    // METODO PARA OBTENER ACTIVAS SEGUN FILTRO
    public async Task<List<Proveedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerProveedoresActivas = _context
            .Proveedores.Where(p => p.EmpresaId == empresaId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerProveedoresActivas = obtenerProveedoresActivas.Where(p =>
                p.Nombre.ToLower().Contains(filtroNormalizado) ||
                p.Cuil.ToLower().Contains(filtroNormalizado) ||
                p.Telefono.ToLower().Contains(filtroNormalizado) ||
                p.Email.ToLower().Contains(filtroNormalizado) ||
                p.Domicilio.ToLower().Contains(filtroNormalizado) ||
                p.Servicio.ToLower().Contains(filtroNormalizado) ||
                p.Observacion.ToLower().Contains(filtroNormalizado) ||
                p.Localidad.Nombre.ToLower().Contains(filtroNormalizado)
            );
        }

        return await obtenerProveedoresActivas.ToListAsync();
    }

    // METODO PARA OBTENER INACTIVAS SEGUN FILTRO
    public async Task<List<Proveedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null)
    {
        var obtenerProveedoresInactivas = _context
            .Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId && p.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            var filtroNormalizado = filtro.Trim().ToLower();
            obtenerProveedoresInactivas = obtenerProveedoresInactivas.Where(p =>
                p.Nombre.ToLower().Contains(filtroNormalizado) ||
                p.Cuil.ToLower().Contains(filtroNormalizado) ||
                p.Telefono.ToLower().Contains(filtroNormalizado) ||
                p.Email.ToLower().Contains(filtroNormalizado) ||
                p.Domicilio.ToLower().Contains(filtroNormalizado) ||
                p.Servicio.ToLower().Contains(filtroNormalizado) ||
                p.Observacion.ToLower().Contains(filtroNormalizado) ||
                p.Localidad.Nombre.ToLower().Contains(filtroNormalizado)
            );
        }

        return await obtenerProveedoresInactivas.ToListAsync();
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<ClienteEstado> ExistePorCuilAsync(string cuil, Guid empresaId)
    {
        bool? estado = await _context
            .Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId && p.Cuil == cuil)
            .Select(p => (bool?)p.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return ClienteEstado.NoExiste;

        return estado.Value ? ClienteEstado.Desactivado : ClienteEstado.Activo;
    }

    // METODO PARA VALIDAR EXISTENCIA EN AGREGAR
    public async Task<ClienteEstado> ExistePorEmailAsync(string email, Guid empresaId)
    {
        bool? estado = await _context
            .Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId && p.Email == email)
            .Select(p => (bool?)p.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return ClienteEstado.NoExiste;

        return estado.Value ? ClienteEstado.Desactivado : ClienteEstado.Activo;
    }

    // METODO PARA VALIDAR EXISTENCIA PARA ACTUALIZAR
    public async Task<ClienteEstado> ExistePorCuilExcluyendoIdAsync(
        string cuil,
        Guid empresaId,
        Guid clienteId
    )
    {
        bool? estado = await _context.Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId && p.Cuil == cuil && p.Id != clienteId)
            .Select(p => (bool?)p.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return ClienteEstado.NoExiste;

        return estado.Value ? ClienteEstado.Desactivado : ClienteEstado.Activo;
    }

    public async Task<ClienteEstado> ExistePorEmailExcluyendoIdAsync(
        string email,
        Guid empresaId,
        Guid clienteId
    )
    {
        bool? estado = await _context.Proveedores.IgnoreQueryFilters()
            .Where(p => p.EmpresaId == empresaId && p.Email.ToLower() == email.ToLower().Trim() && p.Id != clienteId)
            .Select(p => (bool?)p.Eliminado)
            .FirstOrDefaultAsync();

        if (estado == null)
            return ClienteEstado.NoExiste;

        return estado.Value ? ClienteEstado.Desactivado : ClienteEstado.Activo;
    }
}
