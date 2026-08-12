using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Ubicaciones;
using Concesionaria.Domain.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Concesionaria.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUser;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<CategoriaGasto> CategoriasGastos => Set<CategoriaGasto>();
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Provincia> Provincias => Set<Provincia>();
    public DbSet<Localidad> Localidades => Set<Localidad>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<MarcaVehiculo> MarcasVehiculos => Set<MarcaVehiculo>();
    public DbSet<TipoVehiculo> TiposVehiculos => Set<TipoVehiculo>();
    public DbSet<ModeloVehiculo> ModelosVehiculos => Set<ModeloVehiculo>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Empresa>()
            .Property(e => e.MonedaPrincipal)
            .HasConversion<string>();

        builder.Entity<ApplicationUser>()
            .Property(u => u.RolId)
            .HasMaxLength(450);

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Empresa)
            .WithMany()
            .HasForeignKey(u => u.EmpresaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Cuenta>(entity =>
        {
            entity.HasOne(c => c.CuentaPadre)
                .WithMany(c => c.CuentasHijas)
                .HasForeignKey(c => c.CuentaPadreId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");

            entity.Property(u => u.RolId)
                .IsRequired()
                .HasMaxLength(450);

            entity.Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Estado)
                .IsRequired()
                .HasConversion<string>();

            entity.HasIndex(u => new { u.EmpresaId, u.Email })
                .IsUnique();

            entity.HasOne(u => u.Empresa)
                .WithMany()
                .HasForeignKey(u => u.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Sucursal)
                .WithMany()
                .HasForeignKey(u => u.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<IdentityRole>()
                .WithMany()
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        });

            builder.Entity<ModeloVehiculo>()
            .HasOne(m => m.MarcaVehiculo)
            .WithMany()
            .HasForeignKey(m => m.MarcaVehiculoId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ModeloVehiculo>()
            .HasOne(m => m.TipoVehiculo)
            .WithMany()
            .HasForeignKey(m => m.TipoVehiculoId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ModeloVehiculo>()
            .HasOne(m => m.Empresa)
            .WithMany()
            .HasForeignKey(m => m.EmpresaId)
            .OnDelete(DeleteBehavior.NoAction);

        ApplyGlobalFilters(builder);
    }

    private void ApplyGlobalFilters(ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(IHasEmpresa).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(SetGlobalFilter),
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);

                method.Invoke(null, new object[] { builder, _currentUser });
            }
        }
    }

    private static void SetGlobalFilter<TEntity>(
        ModelBuilder builder,
        ICurrentUserService currentUser)
        where TEntity : class, IHasEmpresa
    {
        builder.Entity<TEntity>()
            .HasQueryFilter(x =>
                !EF.Property<bool>(x, "Eliminado"));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var entries = ChangeTracker.Entries<IAuditable>();

        var now = DateTime.UtcNow;
        var userId = _currentUser.UserId;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }
        }
    }
}



