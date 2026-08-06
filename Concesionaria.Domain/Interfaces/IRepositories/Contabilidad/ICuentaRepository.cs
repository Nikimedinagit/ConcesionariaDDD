using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Cuentas.Enums;

public interface ICuentaRepository
{
    //TAREA PARA AGREGAR
    Task AddAsync(Cuenta cuenta);
    //TAREA PARA ACTUALIZAR
    Task UpdateAsync(Cuenta cuenta);
    // TAREA PARA OBTENER ACTIVAS
    Task<List<Cuenta>> ObtenerActivasAsync(
        Guid empresaId,
        string filtro = null,
        TipoCuenta? tipo = null,
        int? nivel = null);
    //TAREA PARA OBTENER INACTIVAS
    Task<List<Cuenta>> ObtenerInactivasAsync(
        Guid empresaId,
        string filtro = null,
        TipoCuenta? tipo = null,
        int? nivel = null);
    //TAREA PARA CONTROL EXISTENCIA AGREGAR
    Task<bool> ExistePorNombreAsync(string nombre, Guid empresaId);
    Task<bool> ExistePorCodigoAsync(string codigo, Guid empresaId);

    //TAREA PARA CONTROL EXISTENCIA ACTUALIZAR
    Task<bool> ExistePorNombreExluyendoIdAsync(
        string nombre,
        Guid empresaId,
        Guid cuentaId
    );
}
