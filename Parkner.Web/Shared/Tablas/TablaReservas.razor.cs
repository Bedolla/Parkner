using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaReservas
    {
        [Parameter]
        public List<Reserva> Reservas { get; set; }
    }
}
