using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionGanancia : IEntityTypeConfiguration<Ganancia>
    {
        public void Configure(EntityTypeBuilder<Ganancia> builder)
        {
            builder.ToTable("Ganancias").HasComment("Beneficios para el prooveedor del servicio por la renta de sus activos");
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id).IsRequired();
            builder.Property(g => g.Fecha).IsRequired().HasColumnType("DateTime2");
            builder.Property(g => g.Cantidad).IsRequired().HasColumnType("Decimal(10,2)");
            builder.Property(g => g.ResponsableId).IsRequired();
            //builder.Property(g => g.Version).IsRowVersion();
        }
    }
}
