using MediatR;

public class AgregarProveedorCommand : IRequest<Guid>
{
    public string Nombre { get; set; }
    public string Cuil { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string Domicilio { get; set; }
    public string Servicio { get; set; }
    public string Observacion { get; set; }
    public Guid LocalidadId { get; set; }
}
