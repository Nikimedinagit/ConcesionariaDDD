using System.Collections.Generic;
using System.Threading.Tasks;

namespace Concesionaria.Domain.Interfaces.IRepositories
{
    public interface IClienteRepository
    {
        // TAREA PARA AGREGAR
        Task AddAsync(Cliente cliente);

        // TAREA PARA ACTUALIZAR
        Task UpdateAsync();

        // TAREA PARA OBTENER ACTIVAS
        Task<List<Cliente>> ObtenerActivasAsync(Guid empresaId, string filtro = null);

        // TAREA PARA OBTENER INACTIVAS
        Task<List<Cliente>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);

        // TAREA PARA EXTIENCIA AGREGAR
        Task<ClienteEstado> ExistePorDniAsync(string dni, Guid empresaId);
        Task<ClienteEstado> ExistePorEmailAsync(string email, Guid empresaId);

        // TAREA PARA EXISTENCIA ACTUALIZAR
        Task<bool> ExistePorDniExcluyendoIdAsync(
            string dni,
            Guid empresaId,
            Guid clienteId
        );
    }
}
