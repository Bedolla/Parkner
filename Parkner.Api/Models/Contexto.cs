using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parkner.Api.Models.Configurations;
using Parkner.Core.Constants;
using Parkner.Data.Entities;
using System;
using System.Collections.Generic;

namespace Parkner.Api.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Responsable> Responsables { get; set; }
        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<Ganancia> Ganancias { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<Direccion> Direcciones { get; set; }
        public virtual DbSet<Horario> Horarios { get; set; }
        public virtual DbSet<Cajon> Cajones { get; set; }
        public virtual DbSet<Estacionamiento> Estacionamientos { get; set; }
        public virtual DbSet<EstacionamientoEmpleado> EstacionamientoEmpleados { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer
                (
                    new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("AppSettings.json")
                        .Build()
                        .GetConnectionString("Local")
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new ConfiguracionUsuario());
            modelBuilder.ApplyConfiguration(new ConfiguracionCliente());
            modelBuilder.ApplyConfiguration(new ConfiguracionResponsable());
            modelBuilder.ApplyConfiguration(new ConfiguracionEmpleado());
            modelBuilder.ApplyConfiguration(new ConfiguracionGanancia());
            modelBuilder.ApplyConfiguration(new ConfiguracionReserva());
            modelBuilder.ApplyConfiguration(new ConfiguracionDireccion());
            modelBuilder.ApplyConfiguration(new ConfiguracionHorario());
            modelBuilder.ApplyConfiguration(new ConfiguracionCajon());
            modelBuilder.ApplyConfiguration(new ConfiguracionEstacionamiento());
            modelBuilder.ApplyConfiguration(new ConfiguracionEstacionamientoEmpleado());

            string direccionA = Guid.NewGuid().ToString();
            string estacionamientoA = Guid.NewGuid().ToString();
            string empleadoA = Guid.NewGuid().ToString();
            string empleadoB = Guid.NewGuid().ToString();
            string empleadoC = Guid.NewGuid().ToString();
            string empleadoD = Guid.NewGuid().ToString();
            string empleadoE = Guid.NewGuid().ToString();
            string responsableA = Guid.NewGuid().ToString();
            string responsableB = Guid.NewGuid().ToString();
            string responsableC = Guid.NewGuid().ToString();
            string responsableD = Guid.NewGuid().ToString();
            string responsableE = Guid.NewGuid().ToString();
            string responsableF = Guid.NewGuid().ToString();

            modelBuilder.Entity<Empleado>().HasData(new List<Empleado>
            {
                new Empleado
                {
                    Id = empleadoA,
                    Nombre = "Empleado",
                    Apellido = "A",
                    Correo = "a@e.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Empleado
                },
                new Empleado
                {
                    Id = empleadoB,
                    Nombre = "Empleado",
                    Apellido = "B",
                    Correo = "b@e.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Empleado
                },
                new Empleado
                {
                    Id = empleadoC,
                    Nombre = "Empleado",
                    Apellido = "C",
                    Correo = "c@e.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Empleado
                },
                new Empleado
                {
                    Id = empleadoD,
                    Nombre = "Empleado",
                    Apellido = "D",
                    Correo = "d@e.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Empleado
                },
                new Empleado
                {
                    Id = empleadoE,
                    Nombre = "Empleado",
                    Apellido = "E",
                    Correo = "e@e.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Empleado
                }
            });

            modelBuilder.Entity<Direccion>().HasData(new List<Direccion>
            {
                new Direccion
                {
                    Id = direccionA,
                    Numero = "1A",
                    Calle = "San Cuilmas",
                    EntreCalles = "San Pedro y San Pablo",
                    Colonia = "San Pancho",
                    CodigoPostal = "28001",
                    Municipio = "Colima",
                    Latitud = "19.239777",
                    Longitud = "-103.729653",
                    EstacionamientoId = estacionamientoA
                }
            });

            modelBuilder.Entity<Estacionamiento>().HasData(new List<Estacionamiento>
            {
                new Estacionamiento
                {
                    Id = estacionamientoA,
                    Nombre = "San Pancho",
                    Descripcion = "Estacionamiento en terreno particular, junto a la plaza principal",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Disponible = true,
                    Tipo = Tipos.Terreno,
                    Creacion = DateTime.Now,
                    Costo = 5M,
                    Concurrido = false,
                    ResponsableId = responsableA
                }
            });

            modelBuilder.Entity<Responsable>().HasData(new List<Responsable>
            {
                new Responsable
                {
                    Id = responsableA,
                    Nombre = "Responsable",
                    Apellido = "A",
                    Correo = "a@r.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Responsable
                },
                new Responsable
                {
                    Id = responsableB,
                    Nombre = "Responsable",
                    Apellido = "B",
                    Correo = "b@r.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Responsable
                },
                new Responsable
                {
                    Id = responsableC,
                    Nombre = "Responsable",
                    Apellido = "C",
                    Correo = "c@r.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Responsable
                },
                new Responsable
                {
                    Id = responsableD,
                    Nombre = "Responsable",
                    Apellido = "D",
                    Correo = "d@r.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Responsable
                },
                new Responsable
                {
                    Id = responsableE,
                    Nombre = "Responsable",
                    Apellido = "E",
                    Correo = "e@r.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Responsable
                },
                new Responsable
                {
                    Id = responsableF,
                    Nombre = "Responsable",
                    Apellido = "F",
                    Correo = "f@r.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Responsable
                }
            });

            modelBuilder.Entity<Cajon>().HasData(new List<Cajon>
            {
                new Cajon
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "A",
                    Disponible = true,
                    EstacionamientoId = estacionamientoA
                },
                new Cajon
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "B",
                    Disponible = true,
                    EstacionamientoId = estacionamientoA
                },
                new Cajon
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "C",
                    Disponible = true,
                    EstacionamientoId = estacionamientoA
                },
                new Cajon
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "D",
                    Disponible = true,
                    EstacionamientoId = estacionamientoA
                },
                new Cajon
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "E",
                    Disponible = true,
                    EstacionamientoId = estacionamientoA
                }
            });

            modelBuilder.Entity<EstacionamientoEmpleado>().HasData(new List<EstacionamientoEmpleado>
            {
                new EstacionamientoEmpleado
                {
                    Id = Guid.NewGuid().ToString(),
                    EstacionamientoId = estacionamientoA,
                    EmpleadoId = empleadoA
                },
                new EstacionamientoEmpleado
                {
                    Id = Guid.NewGuid().ToString(),
                    EstacionamientoId = estacionamientoA,
                    EmpleadoId = empleadoB
                },
                new EstacionamientoEmpleado
                {
                    Id = Guid.NewGuid().ToString(),
                    EstacionamientoId = estacionamientoA,
                    EmpleadoId = empleadoC
                },
                new EstacionamientoEmpleado
                {
                    Id = Guid.NewGuid().ToString(),
                    EstacionamientoId = estacionamientoA,
                    EmpleadoId = empleadoD
                },
                new EstacionamientoEmpleado
                {
                    Id = Guid.NewGuid().ToString(),
                    EstacionamientoId = estacionamientoA,
                    EmpleadoId = empleadoE
                }
            });

            modelBuilder.Entity<Horario>().HasData(new List<Horario>
            {
                new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = "Lunes",
                    DiaNumero = 1,
                    Inicio = new DateTime(2020, 01, 01, 7, 0, 0),
                    Fin = new DateTime(2020, 01, 01, 19, 0, 0),
                    EstacionamientoId = estacionamientoA
                },
                new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = "Martes",
                    DiaNumero = 2,
                    Inicio = new DateTime(2020, 01, 01, 7, 0, 0),
                    Fin = new DateTime(2020, 01, 01, 19, 0, 0),
                    EstacionamientoId = estacionamientoA
                },
                new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = "Miércoles",
                    DiaNumero = 3,
                    Inicio = new DateTime(2020, 01, 01, 7, 0, 0),
                    Fin = new DateTime(2020, 01, 01, 19, 0, 0),
                    EstacionamientoId = estacionamientoA
                },
                new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = "Jueves",
                    DiaNumero = 4,
                    Inicio = new DateTime(2020, 01, 01, 7, 0, 0),
                    Fin = new DateTime(2020, 01, 01, 19, 0, 0),
                    EstacionamientoId = estacionamientoA
                },
                new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = "Viernes",
                    DiaNumero = 5,
                    Inicio = new DateTime(2020, 01, 01, 7, 0, 0),
                    Fin = new DateTime(2020, 01, 01, 19, 0, 0),
                    EstacionamientoId = estacionamientoA
                },
                new Horario
                {
                    Id = Guid.NewGuid().ToString(),
                    Dia = "Sábados",
                    DiaNumero = 6,
                    Inicio = new DateTime(2020, 01, 01, 9, 0, 0),
                    Fin = new DateTime(2020, 01, 01, 16, 0, 0),
                    EstacionamientoId = estacionamientoA
                }
            });

            modelBuilder.Entity<Cliente>().HasData(new List<Cliente>
            {
                new Cliente
                {
                    Id = empleadoA,
                    Nombre = "Cliente",
                    Apellido = "A",
                    Correo = "a@c.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Cliente
                },
                new Cliente
                {
                    Id = empleadoB,
                    Nombre = "Cliente",
                    Apellido = "B",
                    Correo = "b@c.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Cliente
                },
                new Cliente
                {
                    Id = empleadoC,
                    Nombre = "Cliente",
                    Apellido = "C",
                    Correo = "c@c.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Cliente
                },
                new Cliente
                {
                    Id = empleadoD,
                    Nombre = "Cliente",
                    Apellido = "D",
                    Correo = "d@c.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Cliente
                },
                new Cliente
                {
                    Id = empleadoE,
                    Nombre = "Cliente",
                    Apellido = "E",
                    Correo = "e@c.co",
                    Clave = "OGgFiKDvH7RfNOasdZLhfw==",
                    Foto = "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png",
                    Creacion = DateTime.Now,
                    Disponible = true,
                    Rol = Roles.Cliente
                }
            });
        }
    }
}
