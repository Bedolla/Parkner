using System;

namespace Parkner.Web.States
{
    public class UsuarioState
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Correo { get; set; }

        public string Rol { get; set; }

        public string Foto { get; set; }

        public string Clave { get; set; }

        public string Token { get; set; }

        public event Action EnCambioUsuario;

        public void CambioUsuario()
        {
            this.EnCambioUsuario?.Invoke();
        }
    }
}
