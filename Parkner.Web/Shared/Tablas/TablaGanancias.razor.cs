using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaGanancias
    {
        [Parameter]
        public List<Ganancia> Ganancias { get; set; }
    }
}
