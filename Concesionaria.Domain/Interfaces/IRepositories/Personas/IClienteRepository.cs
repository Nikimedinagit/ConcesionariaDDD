using System.Collections.Generic;
using System.Threading.Tasks;
using Concesionaria.Domain.Common.Enums;

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
        Task<EstadoExistencia> ExistePorDniAsync(string dni, Guid empresaId);
        Task<EstadoExistencia> ExistePorEmailAsync(string email, Guid empresaId);

        // TAREA PARA EXISTENCIA ACTUALIZAR
        Task<EstadoExistencia> ExistePorDniExcluyendoIdAsync(
            string dni,
            Guid empresaId,
            Guid clienteId
        );
    
        Task<EstadoExistencia> ExistePorEmailExcluyendoIdAsync(
            string email,
            Guid empresaId,
            Guid clienteId
        );
    }
}
