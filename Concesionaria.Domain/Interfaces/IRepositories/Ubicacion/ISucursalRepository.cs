public interface ISucursalRepository
{
    Task<List<Sucursal>> ObtenerActivasAsync();
    Task<List<Sucursal>> ObtenerInactivasAsync();
}