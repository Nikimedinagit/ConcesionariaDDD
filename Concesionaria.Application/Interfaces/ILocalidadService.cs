// Concesionaria.Application/Interfaces/ILocalidadService.cs
namespace Concesionaria.Application.Interfaces;

public interface ILocalidadService 
{
    Task<IEnumerable<ProvinciaDto>> GetAllProvinciasAsync();
    Task<IEnumerable<LocalidadDto>> GetAllLocalidadesAsync();
}