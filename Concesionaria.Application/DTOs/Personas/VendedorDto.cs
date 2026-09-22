public class VendedorDto
{
    public Guid VendedorId { get; set; }
    public string Nombre { get; set; }
    public string Dni { get; set; }
    public string Email { get; set; }
    public decimal ComisionPorcentaje { get; set; }
    public Guid LocalidadId { get; set; }
    public Guid SucursalId { get; set; }
}