using System.ComponentModel.DataAnnotations.Schema;
using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;

public class Vehiculo : BaseEntity<Guid>, IHasEmpresa, IAuditable, ISoftDelete
{
    public string Version { get; private set; }
    public string Patente { get; private set; }
    public string Color { get; private set; }
    public int Anio { get; private set; }
    public int Kilometraje { get; private set; }
    public CondicionVehiculo Condicion { get; private set; }
    public EstadoVehiculo Estado { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioCompra { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioVenta { get; private set; }
    public bool Eliminado { get; set; }

    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public Guid ModeloId { get; private set; }
    public ModeloVehiculo Modelo { get; private set; }

    public Guid SucursalId { get; private set; }
    public Sucursal Sucursal { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    protected Vehiculo() { }

    private Vehiculo(string version, string patente, string color, int anio, int kilometraje, CondicionVehiculo condicion, EstadoVehiculo estado, decimal precioCompra, decimal precioVenta, Guid modeloId, Guid sucursalId, Guid empresaId)
    {
        Id = Guid.NewGuid();
        Version = version.ToUpper().Trim();
        Patente = patente.ToUpper().Trim();
        Color = color.ToUpper().Trim();
        Anio = anio;
        Kilometraje = kilometraje;
        Condicion = condicion;
        Estado = estado;
        PrecioCompra = precioCompra;
        PrecioVenta = precioVenta;
        ModeloId = modeloId;
        SucursalId = sucursalId;
        EmpresaId = empresaId;
        Eliminado = false;
    }

    public static Vehiculo Crear(string version, string patente, string color, int anio, int kilometraje, CondicionVehiculo condicion, EstadoVehiculo estado, decimal precioCompra, decimal precioVenta, Guid modeloId, Guid sucursalId, Guid empresaId)
    {
        return new Vehiculo(version, patente, color, anio, kilometraje, condicion, estado, precioCompra, precioVenta, modeloId, sucursalId, empresaId);
    }

    public void ActualizarVehiculo(string version, string patente, string color, int anio, int kilometraje, CondicionVehiculo condicion, EstadoVehiculo estado, decimal precioCompra, decimal precioVenta, Guid modeloId, Guid sucursalId)
    {
        Version = version.ToUpper().Trim();
        Patente = patente.ToUpper().Trim();
        Color = color.ToUpper().Trim();
        Anio = anio;
        Kilometraje = kilometraje;
        Condicion = condicion;
        Estado = estado;
        PrecioCompra = precioCompra;
        PrecioVenta = precioVenta;
        ModeloId = modeloId;
        SucursalId = sucursalId;
    }

}
