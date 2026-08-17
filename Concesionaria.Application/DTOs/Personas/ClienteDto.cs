public class ClienteDto
{
    public Guid ClienteId { get; set; }
    public string NombreCompleto { get; set; }
    public string Dni { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Domicilio { get; set; }
    public Guid LocalidadId { get; set; }
}
