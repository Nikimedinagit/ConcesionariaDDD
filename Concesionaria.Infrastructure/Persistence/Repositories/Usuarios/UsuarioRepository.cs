using Concesionaria.Domain.Interfaces.IRepositories;
using Concesionaria.Domain.Usuarios;
using Concesionaria.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> ObtenerActivosAsync(Guid empresaId, string filtro = null)
    {
        var usuarios = _context.Usuarios
            .Where(u => u.EmpresaId == empresaId && !u.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            usuarios = usuarios.Where(u =>
                u.NombreCompleto.Contains(filtro)
                || u.Email.Contains(filtro)
                || u.Sucursal.Nombre.Contains(filtro)
            );
        }

        return await usuarios
            .Include(u => u.Sucursal)
            .ToListAsync();
    }

    public async Task<List<Usuario>> ObtenerInactivosAsync(Guid empresaId, string filtro = null)
    {
        var usuarios = _context.Usuarios
            .IgnoreQueryFilters()
            .Where(u => u.EmpresaId == empresaId && u.Eliminado)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filtro))
        {
            usuarios = usuarios.Where(u =>
                u.NombreCompleto.Contains(filtro)
                || u.Email.Contains(filtro)
                || u.Sucursal.Nombre.Contains(filtro)
            );
        }

        return await usuarios
            .Include(u => u.Sucursal)
            .ToListAsync();
    }

    public async Task<bool> ExisteEmailAsync(string email, Guid empresaId)
    {
        return await _context.Usuarios.AnyAsync(u =>
            u.EmpresaId == empresaId
            && u.Email.ToLower() == email.ToLower()
        );
    }

    public async Task<bool> ExisteEmailExcluyendoIdAsync(
        string email,
        Guid empresaId,
        Guid usuarioId
    )
    {
        return await _context.Usuarios.AnyAsync(u =>
            u.EmpresaId == empresaId
            && u.Id != usuarioId
            && u.Email.ToLower() == email.ToLower()
        );
    }

    public async Task<bool> RolExisteAsync(string rolId)
    {
        return await _context.Roles.AnyAsync(r => r.Id == rolId);
    }

    public async Task<bool> SucursalExisteAsync(Guid sucursalId, Guid empresaId)
    {
        return await _context.Sucursales.AnyAsync(s =>
            s.Id == sucursalId
            && s.EmpresaId == empresaId
            && !s.Eliminado
        );
    }

    public async Task<Dictionary<string, string>> ObtenerRolesAsync()
    {
        return await _context.Roles.ToDictionaryAsync(r => r.Id, r => r.Name);
    }
}
