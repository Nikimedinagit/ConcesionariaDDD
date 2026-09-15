using Concesionaria.Application.Common.Exceptions;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.Common.Models.Imagenes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Concesionaria.Infrastructure.Services.Almacenamiento;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly LocalStorageOptions _options;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _rootPath;

    public LocalFileStorage(
        IOptions<LocalStorageOptions> options,
        IHostEnvironment environment,
        IHttpContextAccessor httpContextAccessor)
    {
        _options = options.Value;
        _httpContextAccessor = httpContextAccessor;
        _rootPath = Path.GetFullPath(
            Path.IsPathRooted(_options.RootPath)
                ? _options.RootPath
                : Path.Combine(environment.ContentRootPath, _options.RootPath));
    }

    public async Task<StoredFile> UploadAsync(
        Stream content,
        string storageKey,
        string contentType,
        long sizeBytes,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (!content.CanRead)
            throw new FileStorageException("No se puede leer el archivo que se desea almacenar.");

        if (string.IsNullOrWhiteSpace(contentType))
            throw new FileStorageException("El tipo de contenido del archivo es obligatorio.");

        if (sizeBytes <= 0)
            throw new FileStorageException("El tamaño del archivo debe ser mayor que cero.");

        var normalizedKey = NormalizeStorageKey(storageKey);
        var targetPath = ResolvePath(normalizedKey);
        var directory = Path.GetDirectoryName(targetPath)!;

        try
        {
            Directory.CreateDirectory(directory);

            await using var destination = new FileStream(
                targetPath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                81920,
                FileOptions.Asynchronous);

            await content.CopyToAsync(destination, cancellationToken);

            return new StoredFile(normalizedKey, contentType, sizeBytes);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            TryDeletePartialFile(targetPath);
            throw new FileStorageException("No se pudo almacenar el archivo.", exception);
        }
    }

    public Task DeleteAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var targetPath = ResolvePath(NormalizeStorageKey(storageKey));

        try
        {
            if (File.Exists(targetPath))
                File.Delete(targetPath);

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            throw new FileStorageException(
                "No se pudo eliminar el archivo almacenado.",
                exception);
        }
    }

    public Task<string> GetReadUrlAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var normalizedKey = NormalizeStorageKey(storageKey);
        var relativeUrl =
            $"{_options.RequestPath.TrimEnd('/')}/{EncodeKey(normalizedKey)}";

        if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
        {
            return Task.FromResult(
                $"{_options.PublicBaseUrl.TrimEnd('/')}{relativeUrl}");
        }

        var request = _httpContextAccessor.HttpContext?.Request;

        if (request is null)
            return Task.FromResult(relativeUrl);

        return Task.FromResult(
            $"{request.Scheme}://{request.Host}{request.PathBase}{relativeUrl}");
    }

    private string ResolvePath(string storageKey)
    {
        var targetPath = Path.GetFullPath(
            Path.Combine(_rootPath, storageKey.Replace('/', Path.DirectorySeparatorChar)));
        var allowedPrefix = _rootPath.TrimEnd(Path.DirectorySeparatorChar) +
                            Path.DirectorySeparatorChar;

        if (!targetPath.StartsWith(allowedPrefix, StringComparison.OrdinalIgnoreCase))
            throw new FileStorageException("La clave de almacenamiento no es válida.");

        return targetPath;
    }

    private static string NormalizeStorageKey(string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            throw new FileStorageException("La clave de almacenamiento es obligatoria.");

        var normalizedKey = storageKey.Trim().Replace('\\', '/').Trim('/');
        var segments = normalizedKey.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length == 0 || segments.Any(segment => segment is "." or ".."))
            throw new FileStorageException("La clave de almacenamiento no es válida.");

        return string.Join('/', segments);
    }

    private static string EncodeKey(string storageKey) =>
        string.Join('/', storageKey.Split('/').Select(Uri.EscapeDataString));

    private static void TryDeletePartialFile(string path)
    {
        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch
        {
            // Conserva el error original de escritura.
        }
    }
}
