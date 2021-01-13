using Blazored.Modal;
using Blazored.Modal.Services;
using Parkner.Web.Shared;
using System;
using System.Threading.Tasks;

namespace Parkner.Web.Helpers.Pages
{
    public interface IMensajes
    {
        Task<bool> MostrarInformacion(string mensaje);
        void MostrarError(string mensaje);
    }

    internal class Mensajes : IMensajes
    {
        public Mensajes(IModalService modal) => this.Modal = modal;

        private IModalService Modal { get; }

        public void MostrarError(string mensaje)
        {
            ModalOptions o = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Position = ModalPosition.Center,
                UseCustomLayout = true
            };

            ModalParameters p = new ModalParameters();
            p.Add(nameof(Confirmar.Tipo), "Error");
            p.Add(nameof(Confirmar.Titulo), "Error");
            p.Add(nameof(Confirmar.Mensaje), mensaje);

            this.Modal.Show<Confirmar>(String.Empty, p, o);
        }

        public async Task<bool> MostrarInformacion(string mensaje)
        {
            ModalOptions o = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Position = ModalPosition.Center,
                UseCustomLayout = true
            };

            ModalParameters p = new ModalParameters();
            p.Add(nameof(Confirmar.Tipo), "Información");
            p.Add(nameof(Confirmar.Titulo), "Información");
            p.Add(nameof(Confirmar.Mensaje), mensaje);

            return !(await this.Modal.Show<Confirmar>(String.Empty, p, o).Result).Cancelled;

        }

        public async Task<ModalResult> MostrarBorrar(string mensaje)
        {
            ModalOptions o = new ModalOptions
            {
                DisableBackgroundCancel = true,
                Position = ModalPosition.Center,
                UseCustomLayout = true
            };

            ModalParameters p = new ModalParameters();
            p.Add(nameof(Confirmar.Titulo), "Borrar");
            p.Add(nameof(Confirmar.Mensaje), mensaje);

            return await this.Modal.Show<Borrar>(String.Empty, p, o).Result;

        }
    }
}
