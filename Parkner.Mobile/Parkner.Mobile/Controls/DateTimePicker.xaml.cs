using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Parkner.Mobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateTimePicker : ContentView
    {
        public static readonly BindableProperty FechaHoraProperty = BindableProperty.Create
        (
            "FechaHora",
            typeof(DateTime),
            typeof(DateTimePicker),
            DateTime.Now,
            BindingMode.TwoWay,
            propertyChanged: DateTimePicker.FechaHoraPropertyChanged
        );

        public static readonly BindableProperty FechaMinimaProperty = BindableProperty.Create
        (
            "FechaMinima",
            typeof(DateTime),
            typeof(DateTimePicker),
            DateTime.Now,
            BindingMode.TwoWay,
            propertyChanged: DateTimePicker.FechaMinimaPropertyChanged
        );

        private string _formato;

        public DateTimePicker()
        {
            this.InitializeComponent();

            //this.Content = new StackLayout
            //{
            //    Children =
            //    {
            //        this.Entrada,
            //        this.SelectorFecha,
            //        this.SelectorHora
            //    }
            //};

            this.SelectorFecha.SetBinding(DatePicker.DateProperty, "Fecha");
            this.SelectorHora.SetBinding(TimePicker.TimeProperty, "Hora");

            this.Entrada.Focused += (t, a) => Device.BeginInvokeOnMainThread(() => this.SelectorFecha.Focus());

            this.SelectorFecha.Focused += (t, a) => this.ActualizarEntrada();
            this.SelectorFecha.Unfocused += (t, a) => Device.BeginInvokeOnMainThread(() =>
            {
                this.SelectorHora.Focus();
                this.Fecha = this.SelectorFecha.Date;
                this.ActualizarEntrada();
            });

            this.SelectorHora.Unfocused += (t, a) => this.Hora = this.SelectorHora.Time;

            this.GestureRecognizers.Add(new TapGestureRecognizer {Command = new Command(() => this.SelectorFecha.Focus())});
        }

        //public Entry Entrada { get; } = new Entry();
        //public DatePicker SelectorFecha { get; } = new DatePicker { IsVisible = false };
        //public TimePicker SelectorHora { get; } = new TimePicker { IsVisible = false, Format = "hh:mm:tt" };

        public string Formato
        {
            get => this._formato ?? "dd/MM/yyyy hh:mm tt";
            set => this._formato = value;
        }

        public DateTime FechaHora
        {
            get => (DateTime)this.GetValue(DateTimePicker.FechaHoraProperty);
            set => this.SetValue(DateTimePicker.FechaHoraProperty, value);
        }

        public DateTime FechaMinima
        {
            get => (DateTime)this.GetValue(DateTimePicker.FechaMinimaProperty);
            set => this.SetValue(DateTimePicker.FechaMinimaProperty, value);
        }

        private TimeSpan Hora
        {
            get => TimeSpan.FromTicks(this.FechaHora.Ticks);
            set => this.FechaHora = new DateTime(this.Fecha.Ticks).AddTicks(value.Ticks);
        }

        private DateTime Fecha
        {
            get => this.FechaHora.Date;
            set => this.FechaHora = new DateTime(this.FechaHora.TimeOfDay.Ticks).AddTicks(value.Ticks);
        }

        private void ActualizarEntrada() => this.Entrada.Text = this.FechaHora.ToString(this.Formato);

        private static void FechaHoraPropertyChanged(BindableObject control, object valorViejo, object valorNuevo) => ((DateTimePicker)control).ActualizarEntrada();

        private static void FechaMinimaPropertyChanged(BindableObject control, object valorViejo, object valorNuevo) => ((DateTimePicker)control).SelectorFecha.MinimumDate = (DateTime)valorNuevo;
    }
}
