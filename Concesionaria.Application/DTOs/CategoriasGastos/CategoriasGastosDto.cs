public record CategoriasGastosDto
{
    public Guid CategoriaGastoId { get; set; }
    public Guid EmpresaId { get; set; }
    public string Nombre { get; set; }
    public bool Eliminado { get; set; }
};
