﻿using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservasDetallesActivaPage : ContentPage
    {
        public ReservasDetallesActivaPage(string id)
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<ReservasDetallesActivaViewModel>(id);
        }
    }
}
