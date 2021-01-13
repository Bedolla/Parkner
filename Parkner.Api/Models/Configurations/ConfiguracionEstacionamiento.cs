using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionEstacionamiento : IEntityTypeConfiguration<Estacionamiento>
    {
        public void Configure(EntityTypeBuilder<Estacionamiento> builder)
        {
            builder.ToTable("Estacionamientos").HasComment("Activos del proveedor del servicio");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Nombre).IsRequired();
            builder.Property(e => e.Descripcion).IsRequired();
            builder.Property(e => e.Costo).IsRequired().HasColumnType("Decimal(10,2)");
            builder.Property(e => e.Foto).IsRequired();
            builder.Property(e => e.Calificacion).HasColumnType("Decimal(5,2)");
            builder.Property(e => e.Creacion).IsRequired().HasColumnType("DateTime2");
            builder.Property(e => e.Disponible).IsRequired();
            builder.Property(e => e.Concurrido).IsRequired();
            builder.Property(e => e.ResponsableId).IsRequired();
            builder.Property(e => e.Tipo).IsRequired();
            //builder.Property(e => e.Version).IsRowVersion();

            builder.HasOne(e => e.Direccion)
                   .WithOne(d => d.Estacionamiento)
                   .HasForeignKey<Direccion>(d => d.EstacionamientoId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Direccion_Estacionamiento");

            builder.HasMany(e => e.Reservas)
                   .WithOne(r => r.Estacionamiento)
                   .HasForeignKey(r => r.EstacionamientoId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .HasConstraintName("FK_Estacionamiento_Reservas");

            builder.HasMany(e => e.Horarios)
                   .WithOne(h => h.Estacionamiento)
                   .HasForeignKey(h => h.EstacionamientoId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Estacionamiento_Horarios");

            builder.HasMany(e => e.Cajones)
                   .WithOne(c => c.Estacionamiento)
                   .HasForeignKey(c => c.EstacionamientoId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Estacionamiento_Cajones");

            builder.HasMany(p => p.Empleados)
                   .WithMany(p => p.Estacionamientos)
                   .UsingEntity<EstacionamientoEmpleado>
                   (
                       r => r
                            .HasOne(ee => ee.Empleado)
                            .WithMany()
                            .HasForeignKey(ee => ee.EmpleadoId),
                       l => l
                            .HasOne(ee => ee.Estacionamiento)
                            .WithMany()
                            .HasForeignKey(pt => pt.EstacionamientoId),
                       j =>
                       {
                           j.Property(ee => ee.Fecha).HasDefaultValueSql("CURRENT_TIMESTAMP");
                           j.HasKey(ee => new { ee.EmpleadoId, ee.EstacionamientoId });
                       }
                   );
        }
    }
}
