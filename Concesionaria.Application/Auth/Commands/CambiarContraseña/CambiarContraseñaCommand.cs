using MediatR;

namespace TuProyecto.Application.Auth.Commands.CambiarContraseña
{
    public class CambiarContraseñaCommand : IRequest<bool>
    {
        public string Contacto { get; set; }      
        public string Token { get; set; }         
        public string NuevaPassword { get; set; } 
    }
}