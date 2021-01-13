using Parkner.Mobile.Services;
using Parkner.Mobile.ViewModels;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Views
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResponsablesEstacionamientoCajonesCrudPage
    {
        public ResponsablesEstacionamientoCajonesCrudPage(string id)
        {
            this.InitializeComponent();

            this.BindingContext = Dependencia.Obtener<ResponsablesEstacionamientoCajonesCrudViewModel>(id);
        }
    }
}
