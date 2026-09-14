namespace Concesionaria.Application.DTOs.Vehiculos;

public sealed record VehiculoImagenDto(
    Guid ImagenId,
    Guid VehiculoId,
    string Url,
    string NombreOriginal,
    string ContentType,
    long TamanioBytes,
    int Ancho,
    int Alto,
    int Orden,
    bool EsPrincipal,
    DateTime CreatedAt);
