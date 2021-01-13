using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionEmpleado : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("Empleados").HasComment("Recepcionistas del servicio/Trabajadores de los Estacionamientos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).IsRequired();
            builder.Property(e => e.Nombre).IsRequired();
            builder.Property(e => e.Apellido).IsRequired();
            builder.Property(e => e.Correo).IsRequired();
            builder.Property(e => e.Clave).IsRequired();
            builder.Property(e => e.Foto).IsRequired();
            builder.Property(e => e.Creacion).IsRequired().HasColumnType("DateTime2");
            builder.Property(e => e.Disponible).IsRequired();
            builder.Property(e => e.Rol).IsRequired();
            //builder.Property(e => e.Version).IsRowVersion();
            builder.Ignore(e => e.Token);
        }
    }
}
