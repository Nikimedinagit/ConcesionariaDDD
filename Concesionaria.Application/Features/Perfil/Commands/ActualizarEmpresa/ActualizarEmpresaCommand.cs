using MediatR;

namespace Concesionaria.Application.Perfil.Commands.ActualizarEmpresa;

public class ActualizarEmpresaCommand : IRequest<bool>
{
    public Guid EmpresaId { get; set; }

    public string NombreFantasia { get; set; } = string.Empty;

    public Guid LocalidadId { get; set; }

    public string Moneda { get; set; } = string.Empty;
}