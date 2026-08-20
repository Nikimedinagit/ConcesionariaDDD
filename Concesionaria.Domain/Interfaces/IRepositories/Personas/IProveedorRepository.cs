public interface IProveedorRepository
{
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Proveedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
    // TAREA PARA OBTENER INACTIVAS
    Task<List<Proveedor>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);
}
