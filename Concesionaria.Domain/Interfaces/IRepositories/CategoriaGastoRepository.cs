using Concesionaria.Domain.CategoriasGastos;

public interface ICategoriaGastoRepository
{
    Task<List<CategoriaGasto>> ObtenerActivasAsync();
    Task<List<CategoriaGasto>> ObtenerInactivasAsync();
}
