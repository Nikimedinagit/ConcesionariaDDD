using MediatR;

namespace Application.Features.Vehiculos.Imagenes.EstablecerPrincipal;

public sealed record EstablecerImagenPrincipalCommand(Guid VehiculoId, Guid ImagenId)
    : IRequest;
