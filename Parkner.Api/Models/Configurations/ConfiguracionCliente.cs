using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionCliente : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes").HasComment("Consumidores del servicio");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Nombre).IsRequired();
            builder.Property(c => c.Apellido).IsRequired();
            builder.Property(c => c.Correo).IsRequired();
            builder.Property(c => c.Clave).IsRequired();
            builder.Property(c => c.Foto).IsRequired();
            builder.Property(c => c.Creacion).IsRequired().HasColumnType("DateTime2");
            builder.Property(c => c.Disponible).IsRequired();
            builder.Property(c => c.Rol).IsRequired();
            //builder.Property(c => c.Version).IsRowVersion();
            builder.Ignore(c => c.Token);

            builder.HasMany(c => c.Reservas)
                   .WithOne(r => r.Cliente)
                   .HasForeignKey(r => r.ClienteId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Cliente_Reservas");
        }
    }
}
