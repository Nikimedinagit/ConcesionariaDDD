namespace Concesionaria.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNotificationAsync(string destino, string mensaje);
}