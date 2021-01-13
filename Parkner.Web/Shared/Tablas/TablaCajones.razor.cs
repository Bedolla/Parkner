using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaCajones
    {
        [Parameter]
        public List<Cajon> Cajones { get; set; }
    }
}
