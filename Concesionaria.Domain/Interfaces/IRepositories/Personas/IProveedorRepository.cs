public interface IProveedorRepository
{
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Proveedor>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
}
