using MediatR;

namespace Application.Features.Vehiculos.Imagenes.ReordenarImagenes;

public sealed record VehiculoImagenOrdenDto(Guid ImagenId, int Orden);

public sealed record ReordenarVehiculoImagenesCommand : IRequest
{
    public Guid VehiculoId { get; init; }
    public IReadOnlyCollection<VehiculoImagenOrdenDto> Imagenes { get; init; } =
        Array.Empty<VehiculoImagenOrdenDto>();
}
