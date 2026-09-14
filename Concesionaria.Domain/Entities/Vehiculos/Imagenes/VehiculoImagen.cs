using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;

public class VehiculoImagen : BaseEntity<Guid>, IHasEmpresa, IAuditable, ISoftDelete
{
    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public Guid VehiculoId { get; private set; }
    public Vehiculo Vehiculo { get; private set; }

    public string StorageKey { get; private set; }
    public string NombreOriginal { get; private set; }
    public string ContentType { get; private set; }
    public long TamanioBytes { get; private set; }
    public int Ancho { get; private set; }
    public int Alto { get; private set; }
    public int Orden { get; private set; }
    public bool EsPrincipal { get; private set; }
    public bool Eliminado { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    protected VehiculoImagen() { }

    private VehiculoImagen(
        Guid empresaId,
        Guid vehiculoId,
        string storageKey,
        string nombreOriginal,
        string contentType,
        long tamanioBytes,
        int ancho,
        int alto,
        int orden,
        bool esPrincipal)
    {
        Id = Guid.NewGuid();
        EmpresaId = empresaId;
        VehiculoId = vehiculoId;
        StorageKey = storageKey.Trim();
        NombreOriginal = nombreOriginal.Trim();
        ContentType = contentType.Trim().ToLowerInvariant();
        TamanioBytes = tamanioBytes;
        Ancho = ancho;
        Alto = alto;
        Orden = orden;
        EsPrincipal = esPrincipal;
        Eliminado = false;
    }

    public static VehiculoImagen Crear(
        Guid empresaId,
        Guid vehiculoId,
        string storageKey,
        string nombreOriginal,
        string contentType,
        long tamanioBytes,
        int ancho,
        int alto,
        int orden,
        bool esPrincipal)
    {
        return new VehiculoImagen(
            empresaId,
            vehiculoId,
            storageKey,
            nombreOriginal,
            contentType,
            tamanioBytes,
            ancho,
            alto,
            orden,
            esPrincipal);
    }

    public void MarcarComoPrincipal() => EsPrincipal = true;

    public void QuitarComoPrincipal() => EsPrincipal = false;

    public void CambiarOrden(int orden) => Orden = orden;

    public void Desactivar() => Eliminado = true;
}
