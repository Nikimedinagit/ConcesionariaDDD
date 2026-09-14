using Amazon.S3;
using Amazon.S3.Model;
using Concesionaria.Application.Common.Exceptions;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.Common.Models.Imagenes;
using Microsoft.Extensions.Options;

namespace Concesionaria.Infrastructure.Services.Almacenamiento;

public sealed class S3FileStorage : IFileStorage
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3StorageOptions _options;

    public S3FileStorage(
        IAmazonS3 s3Client,
        IOptions<S3StorageOptions> options)
    {
        _s3Client = s3Client;
        _options = options.Value;
        ValidateOptions(_options);
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

        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = normalizedKey,
                InputStream = content,
                ContentType = contentType,
                AutoCloseStream = false
            };

            await _s3Client.PutObjectAsync(request, cancellationToken);

            return new StoredFile(normalizedKey, contentType, sizeBytes);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new FileStorageException(
                "No se pudo almacenar el archivo.",
                exception);
        }
    }

    public async Task DeleteAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        var normalizedKey = NormalizeStorageKey(storageKey);

        try
        {
            await _s3Client.DeleteObjectAsync(
                new DeleteObjectRequest
                {
                    BucketName = _options.BucketName,
                    Key = normalizedKey
                },
                cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new FileStorageException(
                "No se pudo eliminar el archivo almacenado.",
                exception);
        }
    }

    public async Task<string> GetReadUrlAsync(
        string storageKey,
        CancellationToken cancellationToken = default)
    {
        var normalizedKey = NormalizeStorageKey(storageKey);

        if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
        {
            return $"{_options.PublicBaseUrl.TrimEnd('/')}/{EncodeKey(normalizedKey)}";
        }

        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _options.BucketName,
                Key = normalizedKey,
                Expires = DateTime.UtcNow.AddMinutes(_options.SignedUrlExpirationMinutes),
                Verb = HttpVerb.GET
            };

            return await _s3Client.GetPreSignedURLAsync(request);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new FileStorageException(
                "No se pudo generar la URL de lectura del archivo.",
                exception);
        }
    }

    private static string NormalizeStorageKey(string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            throw new FileStorageException("La clave de almacenamiento es obligatoria.");

        return storageKey.Trim().Replace('\\', '/').TrimStart('/');
    }

    private static string EncodeKey(string storageKey)
    {
        return string.Join(
            "/",
            storageKey.Split('/').Select(Uri.EscapeDataString));
    }

    private static void ValidateOptions(S3StorageOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BucketName))
            throw new InvalidOperationException("S3Storage:BucketName es obligatorio.");

        if (options.SignedUrlExpirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "S3Storage:SignedUrlExpirationMinutes debe ser mayor que cero.");
        }
    }
}
