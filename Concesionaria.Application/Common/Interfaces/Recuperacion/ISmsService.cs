namespace Concesionaria.Application.Common.Interfaces;

public interface ISmsService
{
    Task EnviarMensajeAsync(string destino, string mensaje);
}