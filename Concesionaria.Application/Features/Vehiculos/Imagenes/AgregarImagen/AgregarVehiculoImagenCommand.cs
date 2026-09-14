using Concesionaria.Application.DTOs.Vehiculos;
using MediatR;

namespace Application.Features.Vehiculos.Imagenes.AgregarImagen;

public sealed record AgregarVehiculoImagenCommand : IRequest<VehiculoImagenDto>
{
    public Guid VehiculoId { get; init; }
    public Stream Content { get; init; }
    public string NombreOriginal { get; init; }
    public string ContentType { get; init; }
    public bool EsPrincipal { get; init; }
}
