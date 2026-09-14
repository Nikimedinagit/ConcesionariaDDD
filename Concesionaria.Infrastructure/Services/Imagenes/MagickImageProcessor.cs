using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Application.Common.Models.Imagenes;
using ImageMagick;
using Microsoft.Extensions.Options;
using ImageProcessingException = Concesionaria.Application.Common.Exceptions.ImageProcessingException;

namespace Concesionaria.Infrastructure.Services.Imagenes;

public sealed class MagickImageProcessor : IImageProcessor
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private static readonly HashSet<MagickFormat> AllowedFormats =
    [
        MagickFormat.Jpeg,
        MagickFormat.Png,
        MagickFormat.WebP
    ];

    private readonly ImageProcessingOptions _options;

    public MagickImageProcessor(IOptions<ImageProcessingOptions> options)
    {
        _options = options.Value;
    }

    public async Task<ProcessedImage> ProcessAsync(
        Stream content,
        string originalFileName,
        string declaredContentType,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(content, originalFileName, declaredContentType);

        await using var input = await CopyWithLimitAsync(content, cancellationToken);

        try
        {
            var imageInfo = new MagickImageInfo(input);

            if (!AllowedFormats.Contains(imageInfo.Format))
            {
                throw new ImageProcessingException(
                    "El archivo debe ser una imagen JPEG, PNG o WebP válida.");
            }

            if ((long)imageInfo.Width * imageInfo.Height > _options.MaxSourcePixels)
            {
                throw new ImageProcessingException(
                    "Las dimensiones de la imagen superan el límite permitido.");
            }

            input.Position = 0;

            using var image = new MagickImage(input);

            image.AutoOrient();

            if (image.Width > _options.MaxWidth || image.Height > _options.MaxHeight)
            {
                var geometry = new MagickGeometry(
                    (uint)_options.MaxWidth,
                    (uint)_options.MaxHeight)
                {
                    IgnoreAspectRatio = false
                };

                image.Resize(geometry);
            }

            image.Strip();
            image.Format = MagickFormat.WebP;
            image.Quality = (uint)_options.WebpQuality;

            var output = new MemoryStream();

            try
            {
                await image.WriteAsync(output, cancellationToken);
                output.Position = 0;

                return new ProcessedImage(
                    output,
                    ".webp",
                    "image/webp",
                    output.Length,
                    checked((int)image.Width),
                    checked((int)image.Height));
            }
            catch
            {
                await output.DisposeAsync();
                throw;
            }
        }
        catch (ImageProcessingException)
        {
            throw;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            throw new ImageProcessingException(
                "No se pudo procesar la imagen. Verificá que el archivo no esté dañado.",
                exception);
        }
    }

    private void ValidateRequest(
        Stream content,
        string originalFileName,
        string declaredContentType)
    {
        ArgumentNullException.ThrowIfNull(content);

        if (!content.CanRead)
            throw new ImageProcessingException("No se puede leer el archivo seleccionado.");

        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new ImageProcessingException("El nombre original de la imagen es obligatorio.");

        if (string.IsNullOrWhiteSpace(declaredContentType) ||
            !AllowedContentTypes.Contains(declaredContentType))
        {
            throw new ImageProcessingException("El formato permitido es JPEG, PNG o WebP.");
        }

        if (_options.MaxInputSizeBytes <= 0)
            throw new InvalidOperationException("MaxInputSizeBytes debe ser mayor que cero.");

        if (_options.MaxWidth <= 0 || _options.MaxHeight <= 0)
            throw new InvalidOperationException("Las dimensiones máximas deben ser mayores que cero.");

        if (_options.MaxSourcePixels <= 0)
            throw new InvalidOperationException("MaxSourcePixels debe ser mayor que cero.");

        if (_options.WebpQuality is < 1 or > 100)
            throw new InvalidOperationException("WebpQuality debe estar entre 1 y 100.");
    }

    private async Task<MemoryStream> CopyWithLimitAsync(
        Stream source,
        CancellationToken cancellationToken)
    {
        var destination = new MemoryStream();
        var buffer = new byte[81920];
        long totalBytes = 0;

        try
        {
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer, cancellationToken)) > 0)
            {
                totalBytes += bytesRead;

                if (totalBytes > _options.MaxInputSizeBytes)
                {
                    throw new ImageProcessingException(
                        $"La imagen no puede superar {_options.MaxInputSizeBytes / 1024 / 1024} MB.");
                }

                await destination.WriteAsync(
                    buffer.AsMemory(0, bytesRead),
                    cancellationToken);
            }

            if (totalBytes == 0)
                throw new ImageProcessingException("La imagen está vacía.");

            destination.Position = 0;
            return destination;
        }
        catch
        {
            await destination.DisposeAsync();
            throw;
        }
    }
}
