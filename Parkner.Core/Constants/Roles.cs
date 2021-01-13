namespace Parkner.Core.Constants
{
    public static class Roles
    {
        public const string Administrador = "Administrador";
        public const string Cliente = "Cliente";
        public const string Empleado = "Empleado";
        public const string Responsable = "Responsable";
        public const string Todos = "Administrador,Cliente,Empleado,Responsable";
        public const string AdministradorCliente = "Administrador,Cliente";
        public const string AdministradorClienteEmpleado = "Administrador,Cliente,Empleado";
        public const string AdministradorEmpleado = "Administrador,Empleado";
        public const string AdministradorEmpleadoResponsable = "Administrador,Empleado,Responsable";
        public const string ClienteEmpleado = "Cliente,Empleado";
        public const string ClienteEmpleadoResponsable = "Cliente,Empleado,Responsable";
        public const string EmpleadoResponsable = "Empleado,Responsable";
        public const string ResponsableCliente = "Responsable,Cliente";
        public const string Soporte = "Soporte";
    }
}
