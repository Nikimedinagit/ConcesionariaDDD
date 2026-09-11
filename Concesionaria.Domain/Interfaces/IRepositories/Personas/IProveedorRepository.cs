using Concesionaria.Domain.Common.Enums;

public interface IProveedorRepository
{
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Proveedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null);

    // TAREA PARA OBTENER INACTIVAS
    Task<List<Proveedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);

    // TAREA PARA AGREGAR
    Task AddAsync(Proveedor proveedor);
    // TAREA PARA VALIDAR EXISTENCIA EN AGREGAR
    Task<EstadoExistencia> ExistePorCuilAsync(string cuil, Guid empresaId);
    Task<EstadoExistencia> ExistePorEmailAsync(string email, Guid empresaId);

    // TAREA PARA EXISTENCIA ACTUALIZAR
        Task<EstadoExistencia> ExistePorCuilExcluyendoIdAsync(
            string cuil,
            Guid empresaId,
            Guid clienteId
        );
    
        Task<EstadoExistencia> ExistePorEmailExcluyendoIdAsync(
            string email,
            Guid empresaId,
            Guid clienteId
        );
}
