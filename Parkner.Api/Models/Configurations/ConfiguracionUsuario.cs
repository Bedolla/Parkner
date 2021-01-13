using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parkner.Core.Constants;
using Parkner.Data.Entities;
using System;

namespace Parkner.Api.Models.Configurations
{
    public class ConfiguracionUsuario : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios").HasComment("Administradores del servicio");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).IsRequired();
            builder.Property(u => u.Nombre).IsRequired();
            builder.Property(u => u.Apellido).IsRequired();
            builder.Property(u => u.Correo).IsRequired();
            builder.Property(u => u.Clave).IsRequired();
            builder.Property(u => u.Foto).IsRequired();
            builder.Property(u => u.Creacion).IsRequired().HasColumnType("DateTime2");
            builder.Property(u => u.Disponible).IsRequired();
            builder.Property(u => u.Rol).IsRequired();
            builder.Ignore(u => u.Token);
            //builder.Property(u => u.Version).IsRowVersion();

            //builder.HasMany(u => u.Reservas)
            //       .WithOne(r => r.Cliente)
            //       .HasForeignKey(r => r.ClienteId)
            //       .OnDelete(DeleteBehavior.Cascade)
            //       .HasConstraintName("FK_Cliente_Reservas");

            //builder.HasMany(u => u.Ganancias)
            //       .WithOne(g => g.Responsable)
            //       .HasForeignKey(g => g.ResponsableId)
            //       .OnDelete(DeleteBehavior.Cascade)
            //       .HasConstraintName("FK_Responsable_Ganancias");

            //builder.HasMany(u => u.Estacionamientos)
            //       .WithOne(e => e.Responsable)
            //       .HasForeignKey(e => e.ResponsableId)
            //       .OnDelete(DeleteBehavior.Cascade)
            //       .HasConstraintName("FK_Responsable_Estacionamientos");

            builder.HasData(new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = "Administrador",
                Apellido = "General",
                Correo = "a@p.co",
                Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                Creacion = DateTime.Now,
                Disponible = true,
                Rol = Roles.Administrador
            });
        }
    }
}
