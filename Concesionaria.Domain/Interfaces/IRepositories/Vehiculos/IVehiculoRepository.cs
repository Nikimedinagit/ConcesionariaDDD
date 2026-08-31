public interface IVehiculoRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(Vehiculo vehiculo);

    //TAREA PARA ACTUALIZAR
    Task UpdateAsync();

    // TAREA PARA OBTENER ACTIVAS
    Task<List<Vehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null);

    // TAREA PARA OBTENER VENDIDOS
    Task<List<Vehiculo>> ObtenerVendidosAsync(Guid empresaId, string filtro = null);

    //TAREA PARA OBTENER INACTIVAS
    Task<bool> ExistePorPatenteAsync(string patente, Guid empresaId);

    //TAREA PARA CONTROL EXISTENCIA ACTUALIZAR
    Task<bool> ExistePorPatenteExluyendoIdAsync(
        string patente,
        Guid empresaId,
        Guid vehiculoId
    );
}
