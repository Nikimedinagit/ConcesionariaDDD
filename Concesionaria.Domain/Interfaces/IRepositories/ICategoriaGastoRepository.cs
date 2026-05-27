using Concesionaria.Domain.CategoriasGastos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Concesionaria.Domain.Interfaces.IRepositories
{
    public interface ICategoriaGastoRepository
    {
        Task AddAsync(CategoriaGasto categoriaGasto);
        Task<List<CategoriaGasto>> ObtenerActivasAsync();
        Task<List<CategoriaGasto>> ObtenerInactivasAsync();
        Task<bool> ExistePorNombreAsync(string nombre);
    }
}
