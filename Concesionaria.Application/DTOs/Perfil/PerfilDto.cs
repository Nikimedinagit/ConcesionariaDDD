public record PerfilDto(
    Guid EmpresaId,
    string RazonSocial,
    string Cuit,
    string NombreFantasia,
    Guid LocalidadId,
    string Telefono,
    string Moneda,
    string Estado,
    string NombreCompleto,
    string Email,
    string AvatarUrl
);