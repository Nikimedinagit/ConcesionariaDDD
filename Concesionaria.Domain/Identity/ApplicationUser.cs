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

    public string? CodigoRecuperacion { get; private set; }
    public DateTime? ExpiracionCodigo { get; private set; }

    protected ApplicationUser() { }

    public ApplicationUser(Guid empresaId, string email, string nombreCompleto)
    {
        EmpresaId = empresaId;
        Email = email;
        UserName = email; 
        NombreCompleto = nombreCompleto;
    }

    public void AsignarRol(Guid rolId) => RolId = rolId;
    public void ActualizarNombre(string nombre) => NombreCompleto = nombre.ToUpper().Trim();
    public void ActualizarAvatar(string url) => AvatarUrl = url;
    public void ActualizarTelefono(string telefono)
        => Telefono = telefono;

        public void EstablecerCodigoRecuperacion(string codigo, int minutosValidez = 5)
    {
        CodigoRecuperacion = codigo;
        ExpiracionCodigo = DateTime.UtcNow.AddMinutes(minutosValidez);
    }

    public void LimpiarCodigoRecuperacion()
    {
        CodigoRecuperacion = null;
        ExpiracionCodigo = null;
    }
}