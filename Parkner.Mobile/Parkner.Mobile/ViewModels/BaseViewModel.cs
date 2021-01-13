using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private ICommand _cablearCommand;
        private ICommand _descablearCommand;
        private bool _ocupado;

        public bool Ocupado
        {
            get => this._ocupado;
            set
            {
                this._ocupado = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand CablearCommand => this._cablearCommand ??= new Command(this.Cablear);

        public ICommand DescablearCommand => this._descablearCommand ??= new Command(this.Descablear);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public virtual void Cablear() { }

        public virtual void Descablear() { }
    }
}
