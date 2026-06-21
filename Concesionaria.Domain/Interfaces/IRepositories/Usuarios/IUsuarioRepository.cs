using Concesionaria.Domain.Usuarios;

namespace Concesionaria.Domain.Interfaces.IRepositories;

public interface IUsuarioRepository
{
    Task<List<Usuario>> ObtenerActivosAsync(Guid empresaId, string filtro = null);

    Task<List<Usuario>> ObtenerInactivosAsync(Guid empresaId, string filtro = null);

    Task<bool> ExisteEmailAsync(string email, Guid empresaId);

    Task<bool> ExisteEmailExcluyendoIdAsync(string email, Guid empresaId, Guid usuarioId);

    Task<bool> RolExisteAsync(string rolId);

    Task<bool> SucursalExisteAsync(Guid sucursalId, Guid empresaId);

    Task<Dictionary<string, string>> ObtenerRolesAsync();
}
