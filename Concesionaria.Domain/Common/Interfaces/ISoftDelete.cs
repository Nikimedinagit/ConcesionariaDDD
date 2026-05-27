namespace Concesionaria.Domain.Common;

public interface ISoftDelete
{
    bool Eliminado { get; set; }
}