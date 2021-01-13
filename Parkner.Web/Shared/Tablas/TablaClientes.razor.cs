using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaClientes
    {
        [Parameter]
        public List<Cliente> Clientes { get; set; }
    }
}
