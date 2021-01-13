using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaResponsables
    {
        [Parameter]
        public List<Responsable> Responsables { get; set; }
    }
}
