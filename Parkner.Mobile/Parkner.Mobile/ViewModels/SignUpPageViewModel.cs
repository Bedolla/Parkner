using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : BaseViewModel
    {
        private string _nombre;
        private string _apellido;
        private string _correo;
        private string _clave;
        private string _claveConfirmar;

        public SignUpPageViewModel()
        {
            this.IngresarCommand = new Command(this.Ingresar);
            this.RegistrarCommand = new Command(this.Registrar);
        }

        public string Nombre
        {
            get => this._nombre;

            set
            {
                if (this._nombre == value) return;

                this._nombre = value;
                this.OnPropertyChanged();
            }
        }

        public string Apellido
        {
            get => this._apellido;

            set
            {
                if (this._apellido == value) return;

                this._apellido = value;
                this.OnPropertyChanged();
            }
        }

        public string Correo
        {
            get => this._correo;

            set
            {
                if (this._correo == value) return;

                this._correo = value;
                this.OnPropertyChanged();
            }
        }

        public string Clave
        {
            get => this._clave;

            set
            {
                if (this._clave == value) return;

                this._clave = value;
                this.OnPropertyChanged();
            }
        }

        public string ClaveConfirmar
        {
            get => this._claveConfirmar;

            set
            {
                if (this._claveConfirmar == value) return;

                this._claveConfirmar = value;
                this.OnPropertyChanged();
            }
        }

        public Command IngresarCommand { get; set; }

        public Command RegistrarCommand { get; set; }

        private void Ingresar(object obj)
        {
            // ToDo: Implementar
        }

        private void Registrar(object obj)
        {
            // ToDo: Implementar
        }
    }
}
