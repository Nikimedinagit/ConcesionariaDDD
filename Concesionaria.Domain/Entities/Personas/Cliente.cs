using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Ubicaciones;

public class Cliente : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public string NombreCompleto { get; private set; }
    public string Dni { get; private set; }
    public string Telefono { get; private set; }
    public string Email { get; private set; }
    public string Domicilio { get; private set; }
    public bool Eliminado { get; set; }
    
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public Guid LocalidadId {get; private set; }
    public Localidad Localidad {get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;


    protected Cliente() { }

    public Cliente(string nombre, string dni, string telefono, string email, string domicilio, Guid localidadId, Guid empresaId)
    {
        Id = Guid.NewGuid();
        NombreCompleto = nombre.ToUpper().Trim();
        Dni = dni;
        Telefono = telefono;
        Email = email;
        Domicilio = domicilio;
        LocalidadId = localidadId;
        EmpresaId = empresaId;
        Eliminado = false;
    }

    public static Cliente Crear(string nombre, string dni, string telefono, string email, string domicilio, Guid localidadId, Guid empresaId)
    {
        return new Cliente(nombre, dni, telefono, email, domicilio, localidadId, empresaId);
    }

    public void ActualizarCliente(string nombre, string dni, string telefono, string email, string domicilio, Guid localidadId)
    {
        NombreCompleto = nombre.ToUpper().Trim();
        Dni = dni.ToUpper().Trim();
        Telefono = telefono.ToUpper().Trim();
        Email = email.ToUpper().Trim();
        Domicilio = domicilio.ToUpper().Trim();
        LocalidadId = localidadId;
    }

    public void Desactivar() => Eliminado = true;
    public void Activar() => Eliminado = false;

}