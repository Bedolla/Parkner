﻿using Microsoft.AspNetCore.Components;
using Parkner.Data.Entities;
using System.Collections.Generic;

namespace Parkner.Web.Shared.Tablas
{
    public partial class TablaUsuarios
    {
        [Parameter]
        public List<Usuario> Usuarios { get; set; }
    }
}
