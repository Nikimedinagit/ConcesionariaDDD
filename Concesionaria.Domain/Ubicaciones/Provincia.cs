using Concesionaria.Domain.Common;

namespace Concesionaria.Domain.Ubicaciones;

public class Provincia : BaseEntity<Guid>, ISoftDelete
{
    public string Nombre { get; private set; }
    public bool Eliminado { get; set; } = false;
    private readonly List<Localidad> _localidades = new();
    public IReadOnlyCollection<Localidad> Localidades => _localidades.AsReadOnly();

    public Provincia(string nombre)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
    }
}