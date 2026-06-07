using Concesionaria.Domain.Cuentas;

public interface ICuentaRepository
{
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Cuenta>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA OBTENER INACTIVAS
    Task<List<Cuenta>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);
}
