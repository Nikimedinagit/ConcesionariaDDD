using Microsoft.EntityFrameworkCore;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Ubicaciones;
using Concesionaria.Domain.CategoriasGastos;
using Concesionaria.Domain.Cuentas;

namespace Concesionaria.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Provincia> Provincias { get; }
    DbSet<Localidad> Localidades { get; }
    DbSet<Empresa> Empresas { get; }
    DbSet<CategoriaGasto> CategoriasGastos { get; }
    DbSet<Cuenta> Cuentas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}