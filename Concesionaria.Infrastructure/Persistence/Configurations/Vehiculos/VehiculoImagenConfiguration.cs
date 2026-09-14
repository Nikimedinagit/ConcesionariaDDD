using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concesionaria.Infrastructure.Persistence.Configurations.Vehiculos;

public class VehiculoImagenConfiguration : IEntityTypeConfiguration<VehiculoImagen>
{
    public void Configure(EntityTypeBuilder<VehiculoImagen> builder)
    {
        builder.ToTable("VehiculoImagenes");

        builder.HasKey(imagen => imagen.Id);

        builder.Property(imagen => imagen.StorageKey)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(imagen => imagen.NombreOriginal)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(imagen => imagen.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(imagen => imagen.TamanioBytes)
            .IsRequired();

        builder.Property(imagen => imagen.Ancho)
            .IsRequired();

        builder.Property(imagen => imagen.Alto)
            .IsRequired();

        builder.Property(imagen => imagen.Orden)
            .IsRequired();

        builder.Property(imagen => imagen.EsPrincipal)
            .IsRequired();

        builder.Property(imagen => imagen.Eliminado)
            .IsRequired();

        builder.HasOne(imagen => imagen.Vehiculo)
            .WithMany(vehiculo => vehiculo.Imagenes)
            .HasForeignKey(imagen => imagen.VehiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(imagen => imagen.Empresa)
            .WithMany()
            .HasForeignKey(imagen => imagen.EmpresaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(imagen => imagen.StorageKey)
            .IsUnique();

        builder.HasIndex(imagen => new
        {
            imagen.EmpresaId,
            imagen.VehiculoId,
            imagen.Orden
        });

        builder.HasIndex(imagen => imagen.VehiculoId)
            .IsUnique()
            .HasFilter("[EsPrincipal] = 1 AND [Eliminado] = 0");
    }
}
