using System.Collections.Generic;
using System.Threading.Tasks;
using Concesionaria.Domain.Common.Enums;

namespace Concesionaria.Domain.Interfaces.IRepositories
{
    public interface ICategoriaGastoRepository
    {
        // TAREA PARA AGREGAR
        Task AddAsync(CategoriaGasto categoriaGasto);

        // TAREA PARA ACTUALIZAR
        Task UpdateAsync();

        // TAREA PARA OBTENER ACTIVAS
        Task<List<CategoriaGasto>> ObtenerActivasAsync(Guid empresaId, string filtro = null);

        // TAREA PARA OBTENER INACTIVAS
        Task<List<CategoriaGasto>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);

        // TAREA PARA EXTIENCIA AGREGAR
        Task<EstadoExistencia> ExistePorNombreAsync(string nombre, Guid empresaId);

        // TAREA PARA EXISTENCIA ACTUALIZAR
        Task<EstadoExistencia> ExistePorNombreExluyendoIdAsync(
            string nombre,
            Guid empresaId,
            Guid CategoriaGastoId
        );
    }
}
