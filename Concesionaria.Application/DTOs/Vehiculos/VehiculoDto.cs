using Concesionaria.Domain.Cuentas.Enums;

public class VehiculoDto
{
    public Guid VehiculoId { get; set; }

    public string Version { get; set; }
    public string Patente { get; set; }
    public string Color { get; set; }

    public int Anio { get; set; }
    public int Kilometraje { get; set; }

    public CondicionVehiculo Condicion { get; set; }
    public EstadoVehiculo Estado { get; set; }

    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }

    public Guid ModeloId { get; set; }
    public string ModeloNombre { get; set; }
    public string MarcaNombre { get; set; }
    public string TipoVehiculoNombre { get; set; }

    public Guid SucursalId { get; set; }
}