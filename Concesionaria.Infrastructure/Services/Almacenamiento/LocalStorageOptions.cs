namespace Concesionaria.Infrastructure.Services.Almacenamiento;

public sealed class LocalStorageOptions
{
    public const string SectionName = "LocalStorage";

    public string RootPath { get; set; } = "wwwroot/uploads";
    public string RequestPath { get; set; } = "/uploads";
    public string PublicBaseUrl { get; set; } = string.Empty;
}
