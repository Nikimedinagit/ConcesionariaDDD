namespace Concesionaria.Infrastructure.Services.Imagenes;

public sealed class ImageProcessingOptions
{
    public const string SectionName = "ImageProcessing";

    public long MaxInputSizeBytes { get; set; } = 5 * 1024 * 1024;
    public int MaxWidth { get; set; } = 1920;
    public int MaxHeight { get; set; } = 1920;
    public long MaxSourcePixels { get; set; } = 40_000_000;
    public int WebpQuality { get; set; } = 82;
}
