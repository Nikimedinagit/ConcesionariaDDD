public interface ITipoVehiculoRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(TipoVehiculo tipoVehiculo);
    //TAREA PARA ACTUALIZAR
    Task UpdateAsync();
    // TAREA PARA OBTENER ACTIVAS
    Task<List<TipoVehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA OBTENER INACTIVAS
    Task<List<TipoVehiculo>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA CONTROL EXISTENCIA AGREGAR
    Task<NombreEntidadVehiculoEstado> ExistePorNombreAsync(string nombre, Guid empresaId);
    //TAREA PARA CONTROL DE RELACION CON MODELOS
    Task<bool> TieneModelosActivosAsync(Guid empresaId, Guid tipoVehiculoId);

    //TAREA PARA CONTROL EXISTENCIA ACTUALIZAR
    Task<NombreEntidadVehiculoEstado> ExistePorNombreExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid tipoVehiculoId
    );
}
