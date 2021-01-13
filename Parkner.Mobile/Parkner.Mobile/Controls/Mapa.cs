using Parkner.Mobile.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Parkner.Mobile.Controls
{
    public class Mapa : Map
    {
        public static readonly BindableProperty PinesProperty = BindableProperty.Create
        (
            nameof(Map.Pins),
            typeof(ObservableCollection<Pin>),
            typeof(Mapa),
            new ObservableCollection<Pin>(),
            propertyChanged: (b, o, n) =>
            {
                Mapa bindable = (Mapa)b;
                bindable.Pins.Clear();

                ObservableCollection<Pin> pines = (ObservableCollection<Pin>)n;
                foreach (Pin pin in pines) bindable.Pins.Add(pin);

                pines.CollectionChanged += (t, a) => Device.BeginInvokeOnMainThread(() =>
                {
                    switch (a.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                        case NotifyCollectionChangedAction.Replace:
                        case NotifyCollectionChangedAction.Remove:
                            if (a.OldItems != null)
                                foreach (object pin in a.OldItems)
                                    bindable.Pins.Remove((Pin)pin);

                            if (a.NewItems != null)
                                foreach (object item in a.NewItems)
                                    bindable.Pins.Add((Pin)item);
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            bindable.Pins.Clear();
                            break;
                    }
                });
            }
        );

        public static readonly BindableProperty PosicionProperty = BindableProperty.Create
        (
            nameof(Mapa.Posicion),
            typeof(Posicion),
            typeof(Mapa),
            new Posicion(0, 0),
            propertyChanged: (bindable, oldvalue, newvalue) => ((Mapa)bindable).MoveToRegion
            (
                MapSpan.FromCenterAndRadius
                (
                    new Position(((Posicion)newvalue).Latitud, ((Posicion)newvalue).Longitud),
                    Distance.FromMeters(((Posicion)newvalue).Distancia)
                )
            ));

        public IList<Pin> Pines { get; set; }

        public Posicion Posicion { get; set; }
    }
}
