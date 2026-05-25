using Concesionaria.Domain.Common;
using Concesionaria.Domain.Empresas.Enums;
using Concesionaria.Domain.Ubicaciones;

namespace Concesionaria.Domain.Empresas;

// Implementamos ISoftDelete para poder "suspender" la empresa
public class Empresa : BaseEntity<Guid>, ISoftDelete
{
    public string RazonSocial { get; private set; }
    public string Cuit { get; private set; }
    public string NombreFantasia { get; private set; }
    public Moneda MonedaPrincipal { get; private set; }
    public Guid LocalidadId { get; private set; }
    public Localidad Localidad { get; private set; }
    public bool Eliminado { get; set; } = false;
    public bool Activa { get; private set; } = true;

    protected Empresa() { }

    private Empresa(string razonSocial, string cuit, string nombreFantasia, Moneda moneda, Localidad localidad)
    {
        Id = Guid.NewGuid();
        RazonSocial = razonSocial;
        Cuit = cuit;
        NombreFantasia = nombreFantasia;
        Localidad = localidad;
        MonedaPrincipal = moneda;
    }

    public static Empresa Crear(string razonSocial, string cuit, string nombreFantasia, Moneda moneda, Localidad localidad)
    {
        return new Empresa(razonSocial, cuit, nombreFantasia, moneda, localidad);
    }

    public void ActualizarNombreFantasia(string nombreFantasia)
    {
        NombreFantasia = nombreFantasia.ToUpper().Trim();
    }

    public void ActualizarMoneda(Moneda moneda)
    {
        MonedaPrincipal = moneda;
    }

    public void ActualizarLocalidad(Guid localidadId)
    {
        LocalidadId = localidadId;
    }

    public void Desactivar()
    {
        Activa = false;
    }

    public void Activar()
    {
        Activa = true;
    }
}