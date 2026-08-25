namespace Concesionaria.Application.Perfil.Queries.GetPerfil;

public class PerfilDto
{

    public Guid EmpresaId { get; set; }

    public string RazonSocial { get; set; } = string.Empty;

    public string Cuit { get; set; } = string.Empty;

    public string NombreFantasia { get; set; } = string.Empty;

    public string Moneda { get; set; } = string.Empty;

    public Guid LocalidadId { get; set; }

    public bool Activa { get; set; }


    public string UsuarioId { get; set; } = string.Empty;

    public string NombreCompleto { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Telefono { get; set; }

    public string AvatarUrl { get; set; }

    public string RolNombre { get; set; } = string.Empty;

    public string SucursalNombre { get; set; } = string.Empty;
}
