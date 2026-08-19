using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISucursalRepository
{
    //TAREA PARA OBTENER ACTIVAS
    Task<List<Sucursal>> ObtenerActivasAsync(Guid empresaId, string filtro);

    //TAREA PARA OBTENER INACTIVAS
    Task<List<Sucursal>> ObtenerInactivasAsync(Guid empresaId, string filtro);

    //TAREA PARA AGREGAR
    Task AddAsync(Sucursal sucursal);

    //TAREA PARA EXISTENCIA AGREGAR NOMBRE
    Task<NombreSucursalEstado> ExistePorNombreLocalidadAsync(
        string nombre,
        Guid empresaId,
        Guid localidadId
    );

    //TAREA PARA EXISTENCIA ACTUALIZAR NOMBRE
    Task<NombreSucursalEstado> ExistePorNombreLocalidadAsync(
        string nombre,
        Guid empresaId,
        Guid localidadId,
        Guid sucursalId
    );

    //TAREA PARA EXISTENCIA ACTUALIZAR LOCALIDAD
    Task<bool> LocalidadExisteAsync(Guid localidadId);

    //TAREA PARA ACTUALIZAR LOCALIDAD
    Task UpdateAsync();
}
