using Concesionaria.Domain.Common;

namespace Concesionaria.Domain.Ubicaciones;

public class Localidad : BaseEntity<Guid>, ISoftDelete
{
    public Guid ProvinciaId { get; private set; }
    public string Nombre { get; private set; }
    public string CodigoPostal { get; private set; }
    public bool Eliminado { get; set; } = false;
    public Provincia Provincia { get; private set; }

    public Localidad(Guid provinciaId, string nombre, string codigoPostal)
    {
        Id = Guid.NewGuid();
        ProvinciaId = provinciaId;
        Nombre = nombre;
        CodigoPostal = codigoPostal;
    }
}