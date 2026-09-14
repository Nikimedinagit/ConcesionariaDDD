using MediatR;

namespace Application.Features.Vehiculos.Imagenes.EliminarImagen;

public sealed record EliminarVehiculoImagenCommand(Guid VehiculoId, Guid ImagenId)
    : IRequest;
