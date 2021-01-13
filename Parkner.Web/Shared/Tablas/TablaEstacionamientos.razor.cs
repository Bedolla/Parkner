﻿using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaEstacionamientos
    {
        [Parameter]
        public List<Estacionamiento> Estacionamientos { get; set; }
    }
}
