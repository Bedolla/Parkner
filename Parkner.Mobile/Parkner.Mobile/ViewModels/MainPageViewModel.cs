using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    internal class MainPageViewModel
    {
        public MainPageViewModel()
        {
            this.MensajeCommand = new Command(async () => await Application.Current.MainPage.DisplayAlert("Aviso", $"La fesha es {this.Fecha}", "Entendido"));

            this.FechaMinima = new DateTime(2020, 11, 29);
        }

        public ICommand MensajeCommand { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaMinima { get; set; }
    }
}
