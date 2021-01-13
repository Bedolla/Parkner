using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionEstacionamientoEmpleado : IEntityTypeConfiguration<EstacionamientoEmpleado>
    {
        public void Configure(EntityTypeBuilder<EstacionamientoEmpleado> builder)
        {
            builder.ToTable("EstacionamientosEmpleados").HasComment("Empleados del Estacionamientos/Estacionamientos del empleado");
            //builder.HasKey(ee => new { ee.EstacionamientoId, ee.EmpleadoId });

            //builder.Property(ee => ee.Version).IsRowVersion();
            builder.Ignore(ee => ee.Id);

            //builder
            //    .HasOne(ee => ee.Estacionamiento)
            //    .WithMany(e => e.Empleados)
            //    .HasForeignKey(ee => ee.EstacionamientoId)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .HasConstraintName("FK_Estacionamiento_EstacionamientosEmpleados");

            //builder
            //    .HasOne(ee => ee.Empleado)
            //    .WithMany(e => e.Estacionamientos)
            //    .HasForeignKey(eh => eh.EmpleadoId)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("FK_Empleado_EstacionamientosEmpleados");
        }
    }
}
