using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionResponsable : IEntityTypeConfiguration<Responsable>
    {
        public void Configure(EntityTypeBuilder<Responsable> builder)
        {
            builder.ToTable("Responsables").HasComment("Proveedores del servicio/Responsables de Estacionamientos y de sus Empleados");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).IsRequired();
            builder.Property(r => r.Nombre).IsRequired();
            builder.Property(r => r.Apellido).IsRequired();
            builder.Property(r => r.Correo).IsRequired();
            builder.Property(r => r.Clave).IsRequired();
            builder.Property(r => r.Foto).IsRequired();
            builder.Property(r => r.Creacion).IsRequired().HasColumnType("DateTime2");
            builder.Property(r => r.Disponible).IsRequired();
            builder.Property(r => r.Rol).IsRequired();
            //builder.Property(r => r.Version).IsRowVersion();
            builder.Ignore(r => r.Token);

            builder.HasMany(r => r.Ganancias)
                   .WithOne(g => g.Responsable)
                   .HasForeignKey(g => g.ResponsableId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Responsable_Ganancias");

            builder.HasMany(r => r.Estacionamientos)
                   .WithOne(e => e.Responsable)
                   .HasForeignKey(e => e.ResponsableId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Responsable_Estacionamientos");
        }
    }
}
