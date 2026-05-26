using Concesionaria.Domain.Cuentas.Enums;

public record CuentaDto
{
    public Guid CuentaId { get; set; }
    public Guid EmpresaId { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public TipoCuenta Tipo { get; set; }
    public int Nivel { get; set; }
    public bool Eliminado { get; set; }
};