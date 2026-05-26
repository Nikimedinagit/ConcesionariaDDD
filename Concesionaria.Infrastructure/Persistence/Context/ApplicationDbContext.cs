using Concesionaria.Infrastructure.Persistence;
using Concesionaria.Domain.Empresas;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Concesionaria.Domain.Ubicaciones;
using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.CategoriasGastos;

namespace Concesionaria.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<CategoriaGasto> CategoriasGastos => Set<CategoriaGasto>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Provincia> Provincias => Set<Provincia>();
    public DbSet<Localidad> Localidades => Set<Localidad>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Mapeo de Empresa
        builder.Entity<Empresa>()
            .Property(e => e.MonedaPrincipal)
            .HasConversion<string>();

        builder.Entity<ApplicationUser>()
        .HasOne(u => u.Empresa) 
        .WithMany()            
        .HasForeignKey(u => u.EmpresaId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Restrict);
    }
}