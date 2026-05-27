using Concesionaria.Domain.Cuentas;

public interface ICuentaRepository
{
    Task<List<Cuenta>> ObtenerActivasAsync();
    Task<List<Cuenta>> ObtenerInactivasAsync();
}
