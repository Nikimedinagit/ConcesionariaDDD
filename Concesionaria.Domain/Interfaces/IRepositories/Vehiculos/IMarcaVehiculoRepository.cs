public interface IMarcaVehiculoRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(MarcaVehiculo marcaVehiculo);
    //TAREA PARA ACTUALIZAR
    Task UpdateAsync();
    // TAREA PARA OBTENER ACTIVAS
    Task<List<MarcaVehiculo>> ObtenerActivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA OBTENER INACTIVAS
    Task<List<MarcaVehiculo>> ObtenerInactivasAsync(Guid empresaId, string filtro = null);
    //TAREA PARA CONTROL EXISTENCIA AGREGAR
    Task<NombreEntidadVehiculoEstado> ExistePorNombreAsync(string nombre, Guid empresaId);
    //TAREA PARA CONTROL DE RELACION CON MODELOS
    Task<bool> TieneModelosActivosAsync(Guid empresaId, Guid marcaVehiculoId);
    //TAREA PARA CONTROL EXISTENCIA ACTUALIZAR
    Task<NombreEntidadVehiculoEstado> ExistePorNombreExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid marcaVehiculoId
    );
}
