public interface IVehiculoRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(Vehiculo vehiculo);

    //TAREA PARA ACTUALIZAR
    Task UpdateAsync();

    // TAREA PARA OBTENER DISONIBLES
    Task<List<Vehiculo>> ObtenerDisponiblesAsync(
        Guid empresaId,
        Guid? sucursalId,
        string filtro = null);

    // TAREA PARA OBTENER RESERVADOS
    Task<List<Vehiculo>> ObtenerReservadosAsync(
        Guid empresaId,
        Guid? sucursalId,
        string filtro = null);
    // TAREA PARA OBTENER EN SERVICI0O
    Task<List<Vehiculo>> ObtenerEnServicioAsync(
        Guid empresaId,
        Guid? sucursalId,
        string filtro = null);

    // TAREA PARA OBTENER VENDIDOS
    Task<List<Vehiculo>> ObtenerVendidosAsync(
        Guid empresaId,
        Guid? sucursalId,
        string filtro = null);

    //TAREA PARA OBTENER INACTIVAS
    Task<bool> ExistePorPatenteAsync(string patente, Guid empresaId);

    //TAREA PARA CONTROL EXISTENCIA ACTUALIZAR
    Task<bool> ExistePorPatenteExluyendoIdAsync(
        string patente,
        Guid empresaId,
        Guid vehiculoId
    );

    //TAREA PARA OBTENER POR MODELO ID
    Task<bool> ObtenerPorModeloIdAsync(Guid modeloId, Guid empresaId);
}
