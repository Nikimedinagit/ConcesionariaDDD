using Concesionaria.Infrastructure.Persistence.Identity;
using Concesionaria.Domain.Empresas;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Concesionaria.Domain.Ubicaciones;

namespace Concesionaria.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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

        // Relación: Usuario -> Empresa
        // IdentityUser ya viene con Id (string), así que vinculamos EmpresaId
        builder.Entity<ApplicationUser>()
            .HasOne<Empresa>()
            .WithMany()
            .HasForeignKey(u => u.EmpresaId)
            .IsRequired();
    }
}