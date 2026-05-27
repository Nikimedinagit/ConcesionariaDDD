using Concesionaria.Application.Common.Interfaces; 
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.Application.Empresas.Queries;

public class GetPerfilQueryHandler
{
    private readonly IApplicationDbContext _context; 

    public GetPerfilQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PerfilDto> Handle(Guid usuarioId)
    {
        var usuario = await _context.Users
        .AsNoTracking()
        .Include(u => u.Empresa) 
            .ThenInclude(e => e.Localidad)
        .FirstOrDefaultAsync(u => u.Id == usuarioId.ToString());

        if (usuario is null)
            throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

        return new PerfilDto(
            EmpresaId: usuario.Empresa.Id,
            RazonSocial: usuario.Empresa.RazonSocial,
            Cuit: usuario.Empresa.Cuit,
            NombreFantasia: usuario.Empresa.NombreFantasia,
            LocalidadId: usuario.Empresa.LocalidadId,
            Moneda: usuario.Empresa.MonedaPrincipal.ToString(),
            Estado: usuario.Empresa.Activa ? "Activo" : "Suspendido",
            NombreCompleto: usuario.NombreCompleto,
            Email: usuario.Email,
            AvatarUrl: usuario.AvatarUrl ?? "/avatars/default.png",
            Telefono: usuario.Telefono
        );
    }
}