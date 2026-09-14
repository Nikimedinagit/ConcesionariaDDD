namespace Concesionaria.Application.Common.Models.Imagenes;

public sealed record StoredFile(
    string StorageKey,
    string ContentType,
    long SizeBytes);
