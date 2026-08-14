using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;

public class Proveedor : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public string Nombre { get; private set; }
    public string Cuil { get; private set; }
    public string Telefono { get; private set; }
    public string Email { get; private set; }
    public string Domicilio { get; private set; }
    public string Servicio { get; private set; }
    public string Observacion { get; private set; }
    public bool Eliminado { get; set; }

    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;


    protected Proveedor() { }

    public Proveedor(string nombre, string cuil, string telefono, string email, string domicilio, string servicio, string observacion, Guid empresaId)
    {
        Id = Guid.NewGuid();
        Nombre = nombre.ToUpper().Trim();
        Cuil = cuil;
        Telefono = telefono;
        Email = email;
        Domicilio = domicilio;
        Servicio = servicio;
        Observacion = observacion;
        EmpresaId = empresaId;
        Eliminado = false;
    }

    public static Proveedor Crear(string nombre, string cuil, string telefono, string email, string domicilio, string servicio, string observacion, Guid empresaId)
    {
        return new Proveedor(nombre, cuil, telefono, email, domicilio, servicio, observacion, empresaId);
    }

    public void ActualizarProveedor(string nombre, string cuil, string telefono, string email, string domicilio, string servicio, string observacion)
    {
        Nombre = nombre.ToUpper().Trim();
        Cuil = cuil.ToUpper().Trim();
        Telefono = telefono.ToUpper().Trim();
        Email = email.ToUpper().Trim();
        Domicilio = domicilio.ToUpper().Trim();
        Servicio = servicio.ToUpper().Trim();
        Observacion = observacion.ToUpper().Trim();
    }

    public void Desactivar() => Eliminado = true;
    public void Activar() => Eliminado = false;

}