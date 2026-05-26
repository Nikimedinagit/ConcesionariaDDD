using MediatR;
namespace Concesionaria.Application.Auth.Commands.SolicitarCodigo;

public class SolicitarCodigoCommand : IRequest<bool>
{
    public string Contacto { get; set; } = string.Empty;
}