public interface IModeloVehiculoRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(ModeloVehiculo modeloVehiculo);
    //TAREA PARA ACTUALIZAR
    Task UpdateAsync();
    // TAREA PARA OBTENER ACTIVAS
    Task<List<ModeloVehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA OBTENER INACTIVAS
    Task<List<ModeloVehiculo>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA CONTROL EXISTENCIA AGREGAR
    Task<NombreModeloEstado> ExistePorNombreAsync(string nombre, Guid empresaId);

    //TAREA PARA CONTROL EXISTENCIA ACTUALIZAR
    Task<bool> ExistePorNombreExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid modeloVehiculoId
    );
}
