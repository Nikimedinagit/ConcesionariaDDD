using Concesionaria.Application.DTOs.Vehiculos;
using MediatR;

namespace Application.Features.Vehiculos.Imagenes.ObtenerImagenes;

public sealed record ObtenerVehiculoImagenesQuery(Guid VehiculoId)
    : IRequest<List<VehiculoImagenDto>>;
