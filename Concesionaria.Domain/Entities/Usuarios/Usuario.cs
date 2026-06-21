using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Usuarios.Enums;

namespace Concesionaria.Domain.Usuarios;

public class Usuario : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public string RolId { get; private set; }
    public Guid SucursalId { get; private set; }
    public Sucursal Sucursal { get; private set; }

    public string NombreCompleto { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public DateTime FechaAlta { get; private set; }
    public DateTime? UltimoAcceso { get; private set; }
    public EstadoUsuario Estado { get; private set; }
    public bool Eliminado { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    protected Usuario() { }

    private Usuario(
        Guid empresaId,
        string rolId,
        Guid sucursalId,
        string nombreCompleto,
        string email,
        string passwordHash
    )
    {
        Id = Guid.NewGuid();
        EmpresaId = empresaId;
        RolId = rolId;
        SucursalId = sucursalId;
        NombreCompleto = nombreCompleto.ToUpper().Trim();
        Email = email.ToLower().Trim();
        PasswordHash = passwordHash;
        FechaAlta = DateTime.UtcNow;
        Estado = EstadoUsuario.Activo;
        Eliminado = false;
    }

    public static Usuario Crear(
        Guid empresaId,
        string rolId,
        Guid sucursalId,
        string nombreCompleto,
        string email,
        string passwordHash
    )
    {
        return new Usuario(
            empresaId,
            rolId,
            sucursalId,
            nombreCompleto,
            email,
            passwordHash
        );
    }

    public void ActualizarDatos(string rolId, Guid sucursalId, string nombreCompleto, string email)
    {
        RolId = rolId;
        SucursalId = sucursalId;
        NombreCompleto = nombreCompleto.ToUpper().Trim();
        Email = email.ToLower().Trim();
    }

    public void ActualizarPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void RegistrarAcceso()
    {
        UltimoAcceso = DateTime.UtcNow;
    }

    public void Activar()
    {
        Estado = EstadoUsuario.Activo;
        Eliminado = false;
    }

    public void Desactivar()
    {
        Estado = EstadoUsuario.Inactivo;
        Eliminado = true;
    }

    public void Bloquear()
    {
        Estado = EstadoUsuario.Bloqueado;
        Eliminado = true;
    }
}
