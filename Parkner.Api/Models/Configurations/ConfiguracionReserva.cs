using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionReserva : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.ToTable("Reservas").HasComment("Consumo de Cajones en un Estacionamiento por los Clientes");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id).IsRequired();
            builder.Property(r => r.Tolerancia).IsRequired();
            builder.Property(r => r.Inicio).HasColumnType("DateTime2");
            builder.Property(r => r.Fin).HasColumnType("DateTime2");
            builder.Property(r => r.Cobrado).HasColumnType("Decimal(10,2)");
            builder.Property(r => r.Disponible).IsRequired();
            builder.Property(r => r.ClienteId).IsRequired();
            builder.Property(r => r.EstacionamientoId).IsRequired();
            builder.Property(r => r.EmpleadoInicializadorId);
            builder.Property(r => r.EmpleadoFinalizadorId);
            //builder.Property(r => r.Version).IsRowVersion();

            builder.HasOne(r => r.EmpleadoInicializador)
                   .WithMany(e => e.ReservasIniciadas)
                   .HasForeignKey(r => r.EmpleadoInicializadorId)
                   .HasConstraintName("FK_EmpleadoInicio_Reservas");

            builder.HasOne(r => r.EmpleadoFinalizador)
                   .WithMany(e => e.ReservasFinalizadas)
                   .HasForeignKey(r => r.EmpleadoFinalizadorId)
                   .HasConstraintName("FK_EmpleadoFin_Reservas");
        }
    }
}
