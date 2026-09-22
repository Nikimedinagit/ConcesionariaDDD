using Concesionaria.Domain.Common;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Ubicaciones;
using System.ComponentModel.DataAnnotations.Schema;

public class Vendedor : BaseEntity<Guid>, ISoftDelete, IHasEmpresa, IAuditable
{
    public string NombreCompleto { get; private set; }
    public string Dni { get; private set; }
    public string Email { get; private set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal ComisionPorcentaje { get; private set; }
    public bool Eliminado { get; set; }

    public Guid EmpresaId { get; private set; }
    public Empresa Empresa { get; private set; }

    public Guid LocalidadId { get; private set; }
    public Localidad Localidad { get; private set; }

    public Guid SucursalId { get; private set; }
    public Sucursal Sucursal { get; private set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;

    protected Vendedor() { }

    public Vendedor(
        string nombre,
        string dni,
        string email,
        decimal comisionPorcentaje,
        Guid sucursalId,
        Guid localidadId,
        Guid empresaId
    )
    {
        Id = Guid.NewGuid();
        NombreCompleto = nombre.ToUpper().Trim();
        Dni = dni;
        Email = email;
        ComisionPorcentaje = comisionPorcentaje;
        SucursalId = sucursalId;
        LocalidadId = localidadId;
        EmpresaId = empresaId;
        Eliminado = false;
    }

    public static Vendedor Crear(
        string nombre,
        string dni,
        string email,
        decimal comisionPorcentaje,
        Guid sucursalId,
        Guid localidadId,
        Guid empresaId
    )
    {
        return new Vendedor(nombre, dni, email, comisionPorcentaje, sucursalId, localidadId, empresaId);
    }

    public void ActualizarVendedor(
        string nombre,
        string dni,
        string email,
        decimal comisionPorcentaje,
        Guid sucursalId,
        Guid localidadId
    )
    {
        NombreCompleto = nombre.ToUpper().Trim();
        Dni = dni;
        Email = email;
        ComisionPorcentaje = comisionPorcentaje;
        SucursalId = sucursalId;
        LocalidadId = localidadId;
    }

    public void Desactivar() => Eliminado = true;

    public void Activar() => Eliminado = false;
}
