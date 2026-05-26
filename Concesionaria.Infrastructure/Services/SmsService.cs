using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Concesionaria.Application.Common.Interfaces;

public class SmsService : ISmsService
{
    public async Task EnviarMensajeAsync(string destino, string mensaje)
    {
        string accountSid = "AC454f4b85d717ba3be779782079347819";
        string authToken = "5d37161dde24c680da92e7011f1cc941";
        
        TwilioClient.Init(accountSid, authToken);

        var message = await MessageResource.CreateAsync(
            body: mensaje,
            from: new Twilio.Types.PhoneNumber("+13048323278"), 
            to: new Twilio.Types.PhoneNumber(destino) 
        );
    }
}