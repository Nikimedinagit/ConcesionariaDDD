using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Ubicaciones;

public class Sucursal : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public Empresa Empresa { get; private set; }
    public Guid EmpresaId { get; private set; }
    public string Nombre { get; private set; }
    public string Direccion { get; private set; }
    public Localidad Localidad { get; private set; }
    public Guid LocalidadId { get; private set; }


    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;


    public bool Eliminado { get; set; }

    protected Sucursal() { }

    private Sucursal(string nombre, string direccion, Guid empresaId, Guid localidadId)
    {
        Id = Guid.NewGuid();

        Nombre = nombre.ToUpper().Trim();
        Direccion = direccion.ToUpper().Trim();
        EmpresaId = empresaId;
        LocalidadId = localidadId;
        Eliminado = false;
    }

    public static Sucursal Crear(string nombre, string direccion, Guid empresaId, Guid localidadId)
    {
        return new Sucursal(nombre, direccion, empresaId, localidadId);
    }

    public void ActualizarNombreSucursal(string nombre)
    {
        Nombre = nombre.ToUpper().Trim();
    }

    public void ActualizarDireccionSucursal(string direccion)
    {
        Direccion = direccion.ToUpper().Trim();
    }

    public void ActualizarLocalidadSucursal(Guid localidadId)
    {
        LocalidadId = localidadId;
    }

    public void Desactivar() => Eliminado = true;

    public void Activar() => Eliminado = false;
}
