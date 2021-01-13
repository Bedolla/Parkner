using System;
using System.Globalization;
using Xamarin.Forms;

namespace Parkner.Mobile.Converters
{
    public class SelectedItemEventArgsToSelectedItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ((SelectedItemChangedEventArgs)value).SelectedItem;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
