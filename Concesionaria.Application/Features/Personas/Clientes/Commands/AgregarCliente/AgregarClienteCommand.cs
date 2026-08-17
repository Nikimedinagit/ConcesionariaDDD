using MediatR;

public record AgregarClienteCommand : IRequest<Guid>
{
    public string NombreCompleto { get; init; } = string.Empty;
    public string Dni { get; init; } = string.Empty;
    public string Telefono { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Domicilio { get; init; } = string.Empty;
    public Guid LocalidadId {get; init;}
}
