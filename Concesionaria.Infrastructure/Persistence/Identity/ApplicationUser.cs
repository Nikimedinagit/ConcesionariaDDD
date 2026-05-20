using Microsoft.AspNetCore.Identity;

namespace Concesionaria.Infrastructure.Persistence.Identity;

public class ApplicationUser : IdentityUser
{
    // Campos de Negocio
    public Guid EmpresaId { get; private set; }
    public string NombreCompleto { get; private set; } = string.Empty;
    public Guid? RolId { get; private set; }

    // Constructor vacío para EF
    protected ApplicationUser() { }

    // Constructor para crear el usuario
    public ApplicationUser(Guid empresaId, string email, string nombreCompleto)
    {
        EmpresaId = empresaId;
        Email = email;
        UserName = email; // El username suele ser el email
        NombreCompleto = nombreCompleto;
    }

    // Métodos de negocio (mantienes tu lógica)
    public void AsignarRol(Guid rolId) => RolId = rolId;
    public void ActualizarNombre(string nombre) => NombreCompleto = nombre;
}