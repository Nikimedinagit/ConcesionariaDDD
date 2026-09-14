namespace Concesionaria.Application.Common.Models.Imagenes;

public sealed class ProcessedImage : IDisposable, IAsyncDisposable
{
    public Stream Content { get; }
    public string FileExtension { get; }
    public string ContentType { get; }
    public long SizeBytes { get; }
    public int Width { get; }
    public int Height { get; }

    public ProcessedImage(
        Stream content,
        string fileExtension,
        string contentType,
        long sizeBytes,
        int width,
        int height)
    {
        Content = content;
        FileExtension = fileExtension;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        Width = width;
        Height = height;
    }

    public void Dispose() => Content.Dispose();

    public ValueTask DisposeAsync() => Content.DisposeAsync();
}
