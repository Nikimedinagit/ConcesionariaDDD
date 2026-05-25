using MediatR;

public class ActualizarContraseñaCommand : IRequest<bool> 
{
    public string PasswordActual { get; set; }
    public string PasswordNueva { get; set; }
}