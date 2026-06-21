using System.Net;
using System.Net.Mail;
using Concesionaria.Application.Common.Interfaces;

namespace Concesionaria.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly ISmsService _smsAppService;

    // CONSTRUCTOR CORREGIDO
    public NotificationService(ISmsService smsAppService)
    {
        _smsAppService = smsAppService;
    }

    public async Task SendNotificationAsync(string destino, string mensaje)
    {
        if (destino.Contains("@"))
        {
            await SendEmail(destino, mensaje);
        }
        else
        {
            await _smsAppService.EnviarMensajeAsync(destino, mensaje);
        }
    }

    private async Task SendEmail(string destino, string mensaje)
    {
        using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("337f06da00baa3", "938eac8304bac9"),
            EnableSsl = true
        };
        var mailMessage = new MailMessage("sistema@concesionaria.com", destino, "Código de Recuperación", mensaje);
        await client.SendMailAsync(mailMessage);
    }
}
