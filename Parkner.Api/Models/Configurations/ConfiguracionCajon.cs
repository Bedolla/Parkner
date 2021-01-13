using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionCajon : IEntityTypeConfiguration<Cajon>
    {
        public void Configure(EntityTypeBuilder<Cajon> builder)
        {
            builder.ToTable("Cajones").HasComment("Producto ofertado para el consumo del Cliente");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Nombre).IsRequired();
            builder.Property(c => c.Disponible).IsRequired();
            builder.Property(c => c.EstacionamientoId).IsRequired();
            //builder.Property(c => c.Version).IsRowVersion();
            builder.Ignore(c => c.Respuesta);
        }
    }
}
