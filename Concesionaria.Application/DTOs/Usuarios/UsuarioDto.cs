namespace Concesionaria.Application.DTOs.Usuarios;

public class UsuarioDto
{
    public Guid UsuarioId { get; set; }
    public Guid EmpresaId { get; set; }
    public string RolId { get; set; } = string.Empty;
    public string RolNombre { get; set; } = string.Empty;
    public Guid SucursalId { get; set; }
    public string SucursalNombre { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime FechaAlta { get; set; }
    public DateTime? UltimoAcceso { get; set; }
    public string Estado { get; set; } = string.Empty;
}
