using Concesionaria.Domain.Cuentas;

public interface ICuentaRepository
{
    Task AddAsync(Cuenta cuenta);
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Cuenta>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA OBTENER INACTIVAS
    Task<List<Cuenta>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA CONTROL EXISTENCIA AGREGAR
    Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId);
    Task<bool> ExistePorCodigoAsync(string codigo, Guid empresaId);
}
