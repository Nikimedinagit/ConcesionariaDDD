using Concesionaria.Domain.CategoriasGastos;

public interface ICategoriaGastoRepository
{
    Task<List<CategoriaGasto>>ObtenerActivasAsync();
}