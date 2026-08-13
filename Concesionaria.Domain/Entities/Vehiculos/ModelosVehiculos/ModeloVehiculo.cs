using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;

public class ModeloVehiculo : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public string Nombre { get; private set; }

    public Guid MarcaVehiculoId { get; private set; }
    public MarcaVehiculo MarcaVehiculo { get; private set; }

    public Guid TipoVehiculoId { get; private set; }
    public TipoVehiculo TipoVehiculo { get; private set; }

    public bool Eliminado { get; set; }

    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    protected ModeloVehiculo() { }
    private ModeloVehiculo(string nombre, Guid empresaId, Guid marcaVehiculoId, Guid tipoVehiculoId)
    {
        Id = Guid.NewGuid();

        Nombre = nombre.ToUpper().Trim();
        EmpresaId = empresaId;
        MarcaVehiculoId = marcaVehiculoId;
        TipoVehiculoId = tipoVehiculoId;
        Eliminado = false;
    }

    public static ModeloVehiculo Crear(string nombre, Guid empresaId, Guid marcaVehiculoId, Guid tipoVehiculoId)
    {
        return new ModeloVehiculo(nombre, empresaId, marcaVehiculoId, tipoVehiculoId);
    }

    public void ActualizarModeloVehiculo(string nombre, Guid marcaVehiculoId, Guid tipoVehiculoId)
    {
        Nombre = nombre.ToUpper().Trim();
        MarcaVehiculoId = marcaVehiculoId;
        TipoVehiculoId = tipoVehiculoId;
    }

    public void Desactivar() => Eliminado = true;

    public void Activar() => Eliminado = false;
}