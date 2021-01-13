using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaEmpleados
    {
        [Parameter]
        public List<Empleado> Empleados { get; set; }
    }
}
