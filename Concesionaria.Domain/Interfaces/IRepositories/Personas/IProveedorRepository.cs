public interface IProveedorRepository
{
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Proveedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null);

    // TAREA PARA OBTENER INACTIVAS
    Task<List<Proveedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);

    // TAREA PARA AGREGAR
    Task AddAsync(Proveedor proveedor);
    // TAREA PARA VALIDAR EXISTENCIA EN AGREGAR
    Task<ClienteEstado> ExistePorCuilAsync(string cuil, Guid empresaId);
    Task<ClienteEstado> ExistePorEmailAsync(string email, Guid empresaId);
}
