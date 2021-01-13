using Parkner.Core.Constants;
using System;
using System.IO;

namespace Parkner.Core.Utilities
{
    public static class Imagenes
    {
        public static MemoryStream Menu => new MemoryStream(Convert.FromBase64String(Iconos.Menu));
    }
}
