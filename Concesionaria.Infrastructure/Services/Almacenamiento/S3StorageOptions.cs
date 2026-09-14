namespace Concesionaria.Infrastructure.Services.Almacenamiento;

public sealed class S3StorageOptions
{
    public const string SectionName = "S3Storage";

    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string PublicBaseUrl { get; set; } = string.Empty;
    public int SignedUrlExpirationMinutes { get; set; } = 15;
    public bool ForcePathStyle { get; set; }
}
