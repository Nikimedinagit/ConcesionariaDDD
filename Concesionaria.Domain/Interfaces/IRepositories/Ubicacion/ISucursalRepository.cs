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
    Task<bool> ExistePorNombreLocalidadAsync(string nombre, Guid empresaId);

    //TAREA PARA EXISTENCIA ACTUALIZAR LOCALIDAD
    Task<bool> LocalidadExisteAsync(Guid localidadId);
}
