using Parkner.Data.Entities;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Parkner.Api.Repositories
{
    public static class QueryableExtensions
    {
        public static IQueryable<Empleado> Buscar(this IQueryable<Empleado> empleados, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                empleados :
                empleados.Where
                (e =>
                    e.Nombre.Contains(terminoBuscado) ||
                    e.Apellido.Contains(terminoBuscado) ||
                    e.Correo.Contains(terminoBuscado)
                );

        public static IQueryable<Responsable> Buscar(this IQueryable<Responsable> responsables, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                responsables :
                responsables.Where
                (r =>
                    r.Nombre.Contains(terminoBuscado) ||
                    r.Apellido.Contains(terminoBuscado) ||
                    r.Correo.Contains(terminoBuscado)
                );

        public static IQueryable<Cliente> Buscar(this IQueryable<Cliente> clientes, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                clientes :
                clientes.Where
                (c =>
                    c.Nombre.Contains(terminoBuscado) ||
                    c.Apellido.Contains(terminoBuscado) ||
                    c.Correo.Contains(terminoBuscado)
                );

        public static IQueryable<Usuario> Buscar(this IQueryable<Usuario> usuarios, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                usuarios :
                usuarios.Where
                (u =>
                    u.Nombre.Contains(terminoBuscado) ||
                    u.Apellido.Contains(terminoBuscado) ||
                    u.Correo.Contains(terminoBuscado)
                );

        public static IQueryable<Estacionamiento> Buscar(this IQueryable<Estacionamiento> estacionamientos, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                estacionamientos :
                estacionamientos.Where
                (e =>
                    e.Nombre.Contains(terminoBuscado) ||
                    e.Tipo.Contains(terminoBuscado)
                );

        public static IQueryable<Cajon> Buscar(this IQueryable<Cajon> cajones, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                cajones :
                cajones.Where(c => c.Nombre.Contains(terminoBuscado));

        public static IQueryable<Horario> Buscar(this IQueryable<Horario> cajones, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                cajones :
                cajones.Where(h => h.Dia.Contains(terminoBuscado));

        public static IQueryable<Reserva> Buscar(this IQueryable<Reserva> reservas, string terminoBuscado) =>
            String.IsNullOrWhiteSpace(terminoBuscado) ?
                reservas :
                reservas.Where
                (r =>
                    r.Cliente.Nombre.Contains(terminoBuscado) ||
                    r.Cliente.Apellido.Contains(terminoBuscado)
                );

        public static IQueryable<Empleado> Ordenar(this IQueryable<Empleado> empleados, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return empleados.OrderBy(e => e.Nombre);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Empleado).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? empleados.OrderBy(e => e.Nombre) : empleados.OrderBy(consultaFinal);
        }

        public static IQueryable<Responsable> Ordenar(this IQueryable<Responsable> responsables, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return responsables.OrderBy(e => e.Nombre);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Responsable).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? responsables.OrderBy(e => e.Nombre) : responsables.OrderBy(consultaFinal);
        }

        public static IQueryable<Usuario> Ordenar(this IQueryable<Usuario> usuarios, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return usuarios.OrderBy(e => e.Nombre);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Usuario).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? usuarios.OrderBy(e => e.Nombre) : usuarios.OrderBy(consultaFinal);
        }

        public static IQueryable<Cliente> Ordenar(this IQueryable<Cliente> clientes, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return clientes.OrderBy(e => e.Nombre);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Cliente).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? clientes.OrderBy(e => e.Nombre) : clientes.OrderBy(consultaFinal);
        }

        public static IQueryable<Estacionamiento> Ordenar(this IQueryable<Estacionamiento> estacionamientos, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return estacionamientos.OrderBy(e => e.Nombre);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Estacionamiento).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? estacionamientos.OrderBy(e => e.Nombre) : estacionamientos.OrderBy(consultaFinal);
        }

        public static IQueryable<Ganancia> Ordenar(this IQueryable<Ganancia> ganancias, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return ganancias.OrderBy(e => e.Fecha);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Ganancia).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? ganancias.OrderBy(e => e.Fecha) : ganancias.OrderBy(consultaFinal);
        }

        public static IQueryable<Cajon> Ordenar(this IQueryable<Cajon> cajones, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return cajones.OrderBy(e => e.Nombre);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Cajon).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? cajones.OrderBy(e => e.Nombre) : cajones.OrderBy(consultaFinal);
        }

        public static IQueryable<Horario> Ordenar(this IQueryable<Horario> horarios, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return horarios.OrderBy(e => e.DiaNumero);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Horario).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? horarios.OrderBy(e => e.DiaNumero) : horarios.OrderBy(consultaFinal);
        }

        public static IQueryable<Reserva> Ordenar(this IQueryable<Reserva> reservas, string ordenarPor)
        {
            if (String.IsNullOrWhiteSpace(ordenarPor)) return reservas.OrderBy(e => e.Inicio);

            StringBuilder consulta = new StringBuilder();

            foreach (string por in ordenarPor.Trim().Split(','))
            {
                if (String.IsNullOrWhiteSpace(por)) continue;

                PropertyInfo propiedad = typeof(Reserva).GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(p => p.Name.Equals(por.Split(" ")[0], StringComparison.InvariantCultureIgnoreCase));

                if (propiedad == null) continue;

                consulta.Append($"{propiedad.Name} {(por.EndsWith(" desc") ? "descending" : "ascending")}, ");
            }

            string consultaFinal = consulta.ToString().TrimEnd(',', ' ');
            return String.IsNullOrWhiteSpace(consultaFinal) ? reservas.OrderBy(e => e.Inicio) : reservas.OrderBy(consultaFinal);
        }
    }
}
