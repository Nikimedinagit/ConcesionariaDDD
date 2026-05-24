using Concesionaria.Domain.Empresas;
using Microsoft.AspNetCore.Identity;

namespace Concesionaria.Domain.Identity;


public class ApplicationUser : IdentityUser
{
    public Guid EmpresaId { get; private set; }
    public virtual Empresa Empresa { get; private set; } = null!;
    public string NombreCompleto { get; private set; } = string.Empty;
    public string? Telefono { get; private set; }
    public Guid? RolId { get; private set; }
    public string? AvatarUrl { get; private set; }

    protected ApplicationUser() { }

    public ApplicationUser(Guid empresaId, string email, string nombreCompleto)
    {
        EmpresaId = empresaId;
        Email = email;
        UserName = email; 
        NombreCompleto = nombreCompleto;
    }


    // Métodos de negocio (mantienes tu lógica)
    public void AsignarRol(Guid rolId) => RolId = rolId;
    public void ActualizarNombre(string nombre) => NombreCompleto = nombre;
    public void ActualizarAvatar(string url) => AvatarUrl = url;
    public void ActualizarTelefono(string telefono)
        => Telefono = telefono;
}