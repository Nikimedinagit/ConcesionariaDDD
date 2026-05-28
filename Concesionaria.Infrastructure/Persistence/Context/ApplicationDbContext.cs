using Concesionaria.Application.Common.Interfaces;
using Concesionaria.Domain.Common.Interfaces;
using Concesionaria.Domain.Cuentas;
using Concesionaria.Domain.Empresas;
using Concesionaria.Domain.Identity;
using Concesionaria.Domain.Ubicaciones;
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
    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Provincia> Provincias => Set<Provincia>();
    public DbSet<Localidad> Localidades => Set<Localidad>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Empresa>()
            .Property(e => e.MonedaPrincipal)
            .HasConversion<string>();

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Empresa)
            .WithMany()
            .HasForeignKey(u => u.EmpresaId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

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



