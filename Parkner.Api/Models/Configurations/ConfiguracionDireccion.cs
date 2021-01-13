using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionDireccion : IEntityTypeConfiguration<Direccion>
    {
        public void Configure(EntityTypeBuilder<Direccion> builder)
        {
            builder.ToTable("Direcciones").HasComment("Auxiliar de Estacionamientos");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id).IsRequired();
            builder.Property(d => d.Numero).IsRequired();
            builder.Property(d => d.Calle).IsRequired();
            builder.Property(d => d.EntreCalles).IsRequired();
            builder.Property(d => d.Colonia).IsRequired();
            builder.Property(d => d.CodigoPostal).IsRequired();
            builder.Property(d => d.Municipio).IsRequired();
            builder.Property(d => d.Latitud).IsRequired();
            builder.Property(d => d.Longitud).IsRequired();
            //builder.Property(d => d.Version).IsRowVersion();

            //builder.HasOne(d => d.Estacionamiento)
            //       .WithOne(e => e.Direccion)
            //       .HasForeignKey<Estacionamiento>(e => e.DireccionId)
            //       .OnDelete(DeleteBehavior.Cascade)
            //       .HasConstraintName("FK_Estacionamiento_Direccion");
        }
    }
}
