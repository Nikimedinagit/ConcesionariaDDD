using Concesionaria.Application.Common.Models.Imagenes;

namespace Concesionaria.Application.Common.Interfaces;

public interface IFileStorage
{
    Task<StoredFile> UploadAsync(
        Stream content,
        string storageKey,
        string contentType,
        long sizeBytes,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string storageKey,
        CancellationToken cancellationToken = default);

    Task<string> GetReadUrlAsync(
        string storageKey,
        CancellationToken cancellationToken = default);
}
