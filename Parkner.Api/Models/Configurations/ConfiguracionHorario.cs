using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Data.Entities;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionHorario : IEntityTypeConfiguration<Horario>
    {
        public void Configure(EntityTypeBuilder<Horario> builder)
        {
            builder.ToTable("Horarios").HasComment("Auxiliar de Estacionamientos");
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Id).IsRequired();
            builder.Property(h => h.Dia).IsRequired();
            builder.Property(h => h.DiaNumero).IsRequired();
            builder.Property(h => h.Inicio).HasColumnType("DateTime2");
            builder.Property(h => h.Fin).HasColumnType("DateTime2");
            //builder.Property(h => h.Version).IsRowVersion();
        }
    }
}
