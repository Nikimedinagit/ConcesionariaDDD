using MediatR;
namespace Concesionaria.Application.Auth.Commands.ValidarCodigo;

public class ValidarCodigoCommand : IRequest<string?> 
{
    public string Contacto { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
}