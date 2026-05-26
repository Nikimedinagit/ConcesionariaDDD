using Microsoft.EntityFrameworkCore;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Ubicaciones;
using Concesionaria.Domain.CategoriasGastos;

namespace Concesionaria.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Provincia> Provincias { get; }
    DbSet<Localidad> Localidades { get; }
    DbSet<Empresa> Empresas { get; }
    DbSet<CategoriaGasto> CategoriasGastos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}