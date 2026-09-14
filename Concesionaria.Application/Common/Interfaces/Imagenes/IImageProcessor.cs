using Concesionaria.Application.Common.Models.Imagenes;

namespace Concesionaria.Application.Common.Interfaces;

public interface IImageProcessor
{
    Task<ProcessedImage> ProcessAsync(
        Stream content,
        string originalFileName,
        string declaredContentType,
        CancellationToken cancellationToken = default);
}
