using Concesionaria.Domain.Common;
using Concesionaria.Domain.Empresas.Enums;

namespace Concesionaria.Domain.Empresas;

// Implementamos ISoftDelete para poder "suspender" la empresa
public class Empresa : BaseEntity<Guid>, ISoftDelete
{
    public string RazonSocial { get; private set; }
    public string Cuit { get; private set; }
    public string NombreFantasia { get; private set; }
    public Moneda MonedaPrincipal { get; private set; }
    public bool Eliminado { get; set; } = false; 
    public bool Activa { get; private set; } = true;

    protected Empresa() { }

    private Empresa(string razonSocial, string cuit, string nombreFantasia, Moneda moneda)
    {
        Id = Guid.NewGuid();
        RazonSocial = razonSocial;
        Cuit = cuit;
        NombreFantasia = nombreFantasia;
        MonedaPrincipal = moneda;
    }
}